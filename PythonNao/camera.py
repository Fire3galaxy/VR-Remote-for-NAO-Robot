import time
import vision_definitions
from naoqi import ALProxy
import png

IP = "192.168.137.18"
PORT = 9559

camera = ALProxy("ALVideoDevice", IP, PORT)
camera.unsubscribe("MyModule")
handle = camera.subscribeCamera("MyModule", 2, 
    vision_definitions.kVGA, vision_definitions.kRGBColorSpace, 30)
width = 640
height = 480

allImages = []

for i in range(0,60):
    # Get image
    image = camera.getImageRemote(handle)

    # Save image for saving to png later
    allImages.append(image[6])

    # release image
    camera.releaseImage(handle)

    # Pause between pictures
    # time.sleep(.1)

for i in range(0,60):
    # Create png image file
    imageRGB = open("D:\\NAO_Pictures\\nao_pic" + str(i) + ".png", "wb")
    w = png.Writer(width, height)

    # Convert image array to library's desired format
    rgbArray = []
    for j in range(0, height):
        rgbRow = []
        rowIdx = j*width*3
        for k in range(0, width):
            colIdx = k*3
            try:
                rgbRow += [ord(allImages[i][rowIdx + colIdx]), 
                    ord(allImages[i][rowIdx + colIdx + 1]), 
                    ord(allImages[i][rowIdx + colIdx + 2])]
            except Exception as e:
                print e
                print i, rowIdx, colIdx
                exit()

        rgbArray.append(tuple(rgbRow))

    # Write image array to file
    w.write(imageRGB, rgbArray)
    imageRGB.close()

camera.unsubscribe(handle)