import time
import vision_definitions
from naoqi import ALProxy
import png

IP = "192.168.137.94"
PORT = 9559

# Always start with this
posture = ALProxy("ALRobotPosture", IP, PORT)
posture.goToPosture("Stand", .5)
# posture.goToPosture("Crouch", 1)

# setPosition doesn't seem to be very reliable. Might need to rely on joints and setAngle?
# Other options?
#motion = ALProxy("ALMotion", IP, PORT)
#motion.wakeUp()
#motion.move(1, 0, 0)
#time.sleep(5)
#print motion.getRobotVelocity()
#motion.stopMove()
# motion.moveTo(.25, 0, 0)
# print motion.getPosition("LShoulderRoll", 0, False)

# # Pitch = up/down, Roll = left/right
# effectors = ["RShoulderRoll", "RShoulderPitch", "RElbowRoll"]
# angles = [-1.32, 0, 0]
# motion.setAngles(effectors, angles, .2)

#motion.rest()
