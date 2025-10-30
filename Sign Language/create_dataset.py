import os
import pickle
import cv2
import mediapipe as mp

# Initialize Mediapipe
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=True, min_detection_confidence=0.3, max_num_hands=2)

# Directory containing hand gesture images
DATA_DIR = './data2'

data = []
labels = []

# Define max features (84 = 2 hands × 21 landmarks × 2 coordinates)
MAX_FEATURES = 84  # 2 hands * 21 landmarks * 2 coordinates

for dir_ in os.listdir(DATA_DIR):
    for img_path in os.listdir(os.path.join(DATA_DIR, dir_)):

        img = cv2.imread(os.path.join(DATA_DIR, dir_, img_path))
        img_rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)


        results = hands.process(img_rgb)
        print(f"\n\nClass: {dir_}")
        print(f"Image: {img_path}, Detected Hands: {len(results.multi_hand_landmarks) if results.multi_hand_landmarks else 0}")

        if results.multi_hand_landmarks:
            hand_data_list = []

            for hand_landmarks in results.multi_hand_landmarks:
                data_aux = []
                x_ = []
                y_ = []

                for i in range(len(hand_landmarks.landmark)):
                    x = hand_landmarks.landmark[i].x
                    y = hand_landmarks.landmark[i].y
                    x_.append(x)
                    y_.append(y)

                for i in range(len(hand_landmarks.landmark)):
                    x = hand_landmarks.landmark[i].x
                    y = hand_landmarks.landmark[i].y
                    data_aux.append(x - min(x_))
                    data_aux.append(y - min(y_))

                for idx, hand_landmarks in enumerate(results.multi_hand_landmarks):
                    print(f"Hand {idx + 1}: {[(lm.x, lm.y) for lm in hand_landmarks.landmark]}")

                hand_data_list.append(data_aux)

            # Merge or pad data for two-hand and one-hand gestures
            if len(hand_data_list) == 2:
                combined_data = hand_data_list[0] + hand_data_list[1]  # Merge both hands
            elif len(hand_data_list) == 1:
                combined_data = hand_data_list[0] + [0] * 42  # Pad missing hand with 42 zeros
            else:
                combined_data = [0] * MAX_FEATURES  # No hand detected, store all zeros

            data.append(combined_data)
            labels.append(dir_)

# Save dataset
with open('data2.pickle', 'wb') as f:
    pickle.dump({'data': data, 'labels': labels}, f)

