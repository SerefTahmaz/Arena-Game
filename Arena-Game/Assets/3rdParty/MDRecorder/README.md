# MD Recorder

This package improves the Unity Recorder to be able to record with an FFmpeg encoder. This is based on [Keijiro FFmpeg Recoder](https://github.com/keijiro/FFmpegRecorder).

### Pros
- ✔️ no artifacts or loss of quality **vs** Unity MP4 encoder 🚫
- ✔️ way faster to record **VS** Unity WebM encoder 🚫
- ✔️ much lighter files **VS** Unity ProRes encoder 🚫

### Cons
- 🔇 audio not recorded (easy work-around, see below)

## How to use
- Import the [package](https://drive.google.com/drive/folders/1xjsgwG4MgYWZN-JcA8b9EFUtYCi4IUSy) into Unity
- Open Unity Recorder ```Windows -> General -> Recorder -> Recorder Window```
- Select the MD Recorder ```Add Recorder -> MD Recorder```
- In format
  - Keep the default Format Preset ```H.264 Default (MP4)```. It should be the best for all of our use cases.
  - If the image is flipped, check ```Flip Image```
- **To get sound**
  -  Add an audio recorder ```Add Recorder -> Audio```
  -  Set the same ```File Name``` and ```Path``` as in the MD Recorder
  -  Merge your MP4 and WAV file in After Effects

## Example
MD Recorder - MP4 encoder - 14 MB             |  UNITY Recorder - *MP4 encoder* - 6 MB |
:-------------------------:|:-------------------------:
✔️ Great quality | 🚫 Bad quality
![MDRecorder](GIF/MDRecorder.gif)  |  ![UnityRecorder](GIF/UnityRecorder.gif)
<img src="GIF/MDRecorder.PNG" alt="MDRecorder" width="400"/>  |  <img src="GIF/UnityRecorder.PNG" alt="UnityRecorder" width="400"/>

## Requirements
- Unity 2019.3 or later. If you use Unity 2022 or later please use the [official Unity custom encoder](https://docs.unity3d.com/Packages/com.unity.recorder@4.0/manual/samples-custom-encoder.html) instead which does the same thing but better (hopefully).
- Unity Recorder installed
