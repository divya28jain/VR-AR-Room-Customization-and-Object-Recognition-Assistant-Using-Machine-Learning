Object Detection with Deep Learning for a Virtual Reality Based Training Simulator
----------------------------------------------------------------------------------

Authors:
- M. Fikret Ercan (Singapore Polytechnic)
- Qiankun Liu (Singapore Polytechnic)
- Yasushi Amari (Japan Advanced Institute of Science and Technology)
- Takashi Miyazaki (National Institute of Technology, Nagano College)


Problem Statement:
- VR headsets provide immersive and cost-effective training solutions.
- However, VR headsets block the user’s real-world view, making it difficult to interact with real objects 
  (e.g., steering wheels, buttons, or even the user’s own hands).
- This limitation reduces usability and interactivity of VR simulators, especially for high-risk tasks 
  such as aircraft towing.
- Challenge: How to blend real objects into VR to improve training realism and safety?


Technologies Used:
- Hardware:
  HTC Vive VR headset with front-mounted cameras.
- Software & Tools:
  Unity (game development engine)
  OpenCV (computer vision library)
  Caffe and Darknet (deep learning frameworks)
- Algorithms:
  YOLOv2 (You Only Look Once v2)
  MobileNet-SSD (optimized Single Shot Multibox Detector for mobile/embedded devices)


Methods Used:
- Blending Real and Virtual Worlds:
  Real-world objects (hands, steering wheel) captured by the headset camera.
  Deep learning models detect these objects and overlay them in the VR scene 
  (creating an "Augmented Virtuality" effect).
- Object Detection Experiments:
  Compared traditional CV methods (Haar, SURF, BRIEF) vs. deep learning (YOLOv2, MobileNet-SSD).
  Deep learning significantly outperformed traditional methods in accuracy and robustness.
- Gesture Recognition:
  Added simple hand gestures (Start/Stop signs) for switching between virtual view and real camera view.
  Achieved ~99% accuracy in controlled gesture tests.
- Performance Evaluation:
  YOLOv2 → higher accuracy but slower → reduced VR frame rates.
  MobileNet-SSD → slightly lower accuracy but faster → chosen for final implementation.


Why These Methods Were Used:
- Traditional CV methods (Haar, SURF, BRIEF) failed under changing lighting and gave poor detection rates.
- Deep learning provided robust detection, real-time adaptability, and better-than-human accuracy.
- MobileNet-SSD was lightweight, efficient, and suitable for real-time VR systems with limited computing power.
- Gesture-based controls allowed intuitive switching without removing the headset.


Key Results:
- Object Detection:
  YOLOv2: ~99–100% accuracy on hands and steering wheel, but slower.
  MobileNet-SSD: ~97–99% accuracy with better real-time performance.
- Gesture Recognition:
  Start/Stop gestures detected with ~99% accuracy in lab tests.
  Real-world VR tests achieved ~80% success (variation due to skin tone diversity).
- Training Effectiveness:
  Ten trainees improved in performance (fewer mistakes, faster completion) after repeated simulator sessions.


Contributions and Significance:
- Developed a low-cost, portable VR training system tailored for the aerospace industry (e.g., aircraft towing).
- Solved VR limitation of blocked real-world vision using deep learning object detection.
- Provided an authoring tool enabling trainers to design scenarios without coding skills.
- Demonstrated how combining AI with VR can enhance immersion, reduce risks, and improve training efficiency.


Conclusion:
This study shows that integrating deep learning object detection into VR simulators 
enables users to interact with both virtual and real-world objects, improving realism and usability. 
It offers an effective, scalable, and cost-efficient solution for safety-critical training.

