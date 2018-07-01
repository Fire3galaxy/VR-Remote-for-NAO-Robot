import png

imageFile = open("nao_pic1.txt", "r")
imageContents = imageFile.read()
imageContents = imageContents.split(' ')
print len(imageContents)

# 320 by 240
width = 640
height = 480
rgbArray = []
for j in range(0, height):
    rgbRow = []
    rowIdx = j*width*3
    for i in range(0, width):
        colIdx = i*3
        rgbRow += [int(imageContents[rowIdx + colIdx]), 
            int(imageContents[rowIdx + colIdx + 1]), 
            int(imageContents[rowIdx + colIdx + 2])]
    rgbArray.append(tuple(rgbRow))

imageRGB = open("nao_pic1.png", "wb")
w = png.Writer(width, height)
w.write(imageRGB, rgbArray)

imageFile.close()
imageRGB.close()