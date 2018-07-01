from naoqi import ALProxy
import time
import almath

# Setup data
IP = "192.168.137.18"
PORT = 9559
tts = ALProxy("ALTextToSpeech", IP, PORT)
motion = ALProxy("ALMotion", IP, PORT)

# Motion data
chainNames = ["LArm", "RArm"]
jointNames = ["LShoulderRoll", "RShoulderRoll"]
frame = 0 #motion.FRAME_TORSO
useSensor = False
directionNames = ["down", "forward", "up", "backward", "out"]

# Initial position
current = motion.getPosition(chainNames[0], frame, useSensor)
print "Original (left) position of arm:\n", current
current = motion.getPosition(jointNames[0], frame, useSensor)
print "Original (left) position of shoulder:\n", current

# grabs total range of arm
print "----Left arm----"
for i in range(0, 5):
    print "Go to ", directionNames[i],  # no new line here
    tts.say("Go to " + directionNames[i])
    time.sleep(2)
    current = motion.getPosition(chainNames[0], frame, useSensor)
    print directionNames[i], " position:\n", current

# Initial position
current = motion.getPosition(chainNames[1], frame, useSensor)
print "Original (right) position of arm:\n", current
current = motion.getPosition(jointNames[0], frame, useSensor)
print "Original (left) position of shoulder:\n", current

# grabs total range of arm
print "----Right arm----"
for i in range(0, 5):
    print "Go to ", directionNames[i],  # no new line here
    tts.say("Go to " + directionNames[i])
    time.sleep(2)
    current = motion.getPosition(chainNames[1], frame, useSensor)
    print directionNames[i], " position:\n", current
