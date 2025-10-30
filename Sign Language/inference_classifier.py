import pickle
import os
import socket
import cv2
import mediapipe as mp
import numpy as np
import pyglet
from SpoutGL import SpoutSender
from OpenGL.GL import GL_RGBA
from sklearn.ensemble import RandomForestClassifier

# Load trained model
model_dict = pickle.load(open('./model2.p', 'rb'))
model = model_dict['model']

# Warm up the ML model with a dummy input
dummy_input = np.zeros((1, 84))  # 42 landmarks * 2 hands
model.predict(dummy_input)

# Initialize webcam
cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 960)

# 1. Create hidden pyglet window for OpenGL context
config = pyglet.gl.Config(double_buffer=False)
window = pyglet.window.Window(width=1, height=1, visible=False, config=config)

# 2. Create SpoutSender AFTER window/context is ready
sender = SpoutSender()
sender.setSenderName("inference_classifier")

# Warm up the camera by grabbing some frames
for _ in range(30):
    cap.read()

# Create a dummy frame for Mediapipe warm-up
dummy_frame = np.zeros((480, 640, 3), dtype=np.uint8)

# Initialize Mediapipe
mp_hands = mp.solutions.hands
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles

hands = mp_hands.Hands(static_image_mode=False, min_detection_confidence=0.3, max_num_hands=2)

# Warm up Mediapipe by processing dummy frames
for _ in range(5):
    hands.process(cv2.cvtColor(dummy_frame, cv2.COLOR_BGR2RGB))

# UDP socket setup
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ('127.0.0.1', 1594)

# Label dictionary
labels_dict = {0: 'A', 1: 'B', 2: 'C',3: 'D', 4: 'E', 5: 'F',6: 'G', 7: 'H', 8: 'I',9: 'K', 10: 'L', 11: 'M', 12: 'N', 13: 'O', 14: 'P',15: 'Q', 16: 'R', 17: 'S',18: 'T', 19: 'U', 20: 'V',21: 'W', 22: 'X', 23: 'Y',24: 'Z' }

# Letters that require two hands
two_hand_letters = {'A', 'B','D', 'F','G', 'H','K', 'M','N', 'P', 'Q', 'S','T', 'W','X', 'Y'}  # Example: Modify this list based on the actual dataset.

while True:
    ret, frame = cap.read()
    if not ret:
        continue

    cv2.putText(frame, 'Press Q to Close the Camera', (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 0), 3,
                cv2.LINE_AA)

    H, W, _ = frame.shape
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    results = hands.process(frame_rgb)

    if results.multi_hand_landmarks:
        hand_data_list = []  # Store each hand's data separately

        for hand_landmarks in results.multi_hand_landmarks:
            data_aux = []
            x_ = []
            y_ = []

            # Collect x, y coordinates
            for i in range(len(hand_landmarks.landmark)):
                x = hand_landmarks.landmark[i].x
                y = hand_landmarks.landmark[i].y
                x_.append(x)
                y_.append(y)

            # Normalize coordinates
            for i in range(len(hand_landmarks.landmark)):
                x = hand_landmarks.landmark[i].x
                y = hand_landmarks.landmark[i].y
                data_aux.append(x - min(x_))
                data_aux.append(y - min(y_))

            if len(data_aux) == 42:
                hand_data_list.append(data_aux)

        # If two hands are detected, merge them for two-hand letters
        if len(hand_data_list) == 2:  # Two hands detected
            combined_data = hand_data_list[0] + hand_data_list[1]  # Merge both hands
            try:
                prediction = model.predict([np.asarray(combined_data)])
                predicted_character = labels_dict[int(prediction[0])]
                final_prediction = predicted_character  # No restriction on two-hand letters
            except:
                final_prediction = "?"

        elif len(hand_data_list) == 1:  # One hand detected
            combined_data = hand_data_list[0] + [0] * 42
            try:
                prediction = model.predict([np.asarray(combined_data)])
                predicted_character = labels_dict[int(prediction[0])]
                final_prediction = predicted_character  # Allow one-hand letters like 'C'
            except:
                final_prediction = "IDK"

        else:
            final_prediction = "?"

        # Get bounding box for visualization
        x1 = int(min(x_) * W) - 10
        y1 = int(min(y_) * H) - 10
        x2 = int(max(x_) * W) + 10
        y2 = int(max(y_) * H) + 10

        # Draw landmarks and bounding box
        for hand_landmarks in results.multi_hand_landmarks:
            mp_drawing.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS,
                                      mp_drawing_styles.get_default_hand_landmarks_style(),
                                      mp_drawing_styles.get_default_hand_connections_style())

        cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 0, 0), 4)
        cv2.putText(frame, final_prediction, (x1, y1 - 10), cv2.FONT_HERSHEY_SIMPLEX, 1.3, (0, 0, 0), 3,
                    cv2.LINE_AA)

        # Send prediction over socket
        sock.sendto(str.encode(final_prediction), serverAddressPort)

        # Prepare frame for Spout: make contiguous and send with GL_BGR format
    frame_rgba = cv2.cvtColor(frame, cv2.COLOR_BGR2RGBA)
    frame_data = np.ascontiguousarray(frame_rgba)
    sender.sendImage(frame_data, W, H, GL_RGBA, False, 4)
    cv2.imshow('frame2', frame)

    if cv2.waitKey(1) == ord('q'):
        break

cap.release()
sender.release()
window.close()
cv2.destroyAllWindows()
