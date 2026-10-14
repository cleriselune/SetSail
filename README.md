## Set Sail
MAIN DESIGN CONCEPTS:
- scene: a plane (acting as water), a ship, and two labels as UI
- the ship has a sail opening value from 0 to 100 (percentage) (W/S)
- the ship has one sail and it can rotate it up to 45 degrees clockwise or counterclockwise (A/D)
- the UI consists of only two labels: opening and rotation of sails
- the ship must behave like a ship: drag and wind must be taken into account
- there are no physics or collisions (all moving happens by using transform instead of a rigidbody

https://github.com/user-attachments/assets/fe24baae-232f-4644-af92-3a7b34656b92

WIND MECHANIC:
- wind comes in from the back of the ship at it's starting point (south)
- speed in this position is best while its 100% open sails and 0* sail rotation
- speed lowers when rotation occurs
- going right (west) best speed would be achieved by rotating the sail to -45*
- the same with left (east) and 45*
- while going north (against the wind) speed is almost 0 according to the no-sail zone
  - the no-sail zone: if the bow is pointed directly into the wind or within roughly 30° to 45° on either side into which a sailboat cannot directly sail

DRAG MECHANIC:
- from the calculated propulsion resistance is subtracted
- resistance grows the higher the speed (dragCoeff * currentspeed^2)
- ship won't gain high momentum quickly and also won't stop suddenly after closing sails

STEER MECHANIC:
- added steer mechanic (Q/E) to test the behaviour of the speed to the wind :]

TECHNICAL DETAILS:
- all UI elements are created in UIToolkit
- new input system
- compiler: IL2CPP with maximum stripping and optimization
- URP
- Unity version: 6000.5.11f1
