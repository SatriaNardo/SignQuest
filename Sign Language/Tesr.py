import pickle

data_dict = pickle.load(open('./data2.pickle','rb'))
print(data_dict.keys())
print(data_dict)