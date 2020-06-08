import time	

for y in range(-10, 10):
	str = ""
	for x in range(-10, 10):
		#print(((x*y)//100)*100)

		if (((x*x+y*y)//10)*10 == 10):
			str += "-"
		else:
			str += " "
	print(str)