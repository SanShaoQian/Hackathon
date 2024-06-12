# Team Name: MError
## Description
Welcome to our project! This AR application with advanced image recognition capabilities is designed to provide users with informative and advisory content about the wildlife they encounter during walks in nature reserves or parks. With this app, users can point their phone camera at an animal they come across, and the app will automatically detect the animal and display a screen with relevant information and advice on how to interact with it safely. 
## Features
### Augmented Reality and Image Recognition
- Real-time Animal Detection: Utilizing a Tensor Flow Lite model we have trained ourselves, our app can identify 80 different animal species that users might encounter in nature reserves and parks.
- Informative Overlays: Once an animal is detected, the app displays an overlay with detailed information about the animal, including its habitat, behavior, and safety advice.
- User-Friendly Interface: The app is designed with simplicity in mind, ensuring that users of all ages can easily navigate and use its features.
### Advisory Information
- Safety Guideline: Each detected animal comes with a set of safety guidelines, helping users understand the best practices for interacting with the animal or avoiding potential dangers.
- Educational Content: Users can learn more about the ecology, conservation status, and interesting facts about each animal, enhancing their nature walk experience.

## Challenges we ran into
1. Our biggest challenge was **accurate animal detection** by developing a robust algorithm to accurately detect and identify a wide variety of animals in diverse environments.
2. Real-Time Processing: Ensuring the app processes images and delivers information in real time without significant lag. We managed to solve this by reducing the number of calls to the object detector instead of once per frame.
3. It was also very time-consuming to build an extensive and reliable database of animal facts and safety tips.
4. User Interface Design: Creating an intuitive and user-friendly interface that appeals to a broad audience.
   
## Accomplishments that we're proud of
1. Teamwork and friendship 
2. Training our own model to detect animals
   
## What we learned
1. Importance of User Feedback: Early and continuous testing provided invaluable insights, helping us refine the app's interface and functionality to better meet user needs.
2. Complexity of Real-Time Processing: Real-time image processing and AR integration present significant technical challenges, but with careful optimization and innovation, they can be effectively addressed.
3. Balancing Performance and Resource Usage: Striking a balance between high performance and minimal battery/data consumption is crucial for user satisfaction and app sustainability.
4. Educational Impact: The potential of AR technology to make learning interactive and engaging is immense, reinforcing the value of innovative approaches in education.
## What's next for MError
Here some possible extensions of our project
- Plant Recognition: We plan to expand our app's capabilities to include **plant recognition**, providing users with information about various flora they encounter during their walks.
- Additional Species: Our team is continuously working to increase the number of recognizable animal species, aiming to provide a **comprehensive nature guide**.

## Installation and usage instructions
1. Install the .apk file onto your phone.
2. Permissions: Allow the app to access your camera to enable the image recognition feature.
3. Point it at an animal/ image of an animal to access the information.
   
## Development tools used
1. Unity (ARFoundation - ARCore)
2. TensorFlow Lite
3. Visual Studio Code
4. GitHub

## Licensing (if any)
Pictures and information are taken from Wikipedia Creative Commons
Information from http://ielc.libguides.com/sdzg/factsheets/
