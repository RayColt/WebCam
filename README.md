# WebCam
Webcam JPG Motion Detector<br /><br />
Some Restored University Pepperdine Code I created in 1976, aged 4. To test the new C# and motion tracking.<br />
A cult in short term small computer espionage it became! For extra security program use a directory Watcher with it like the one in the directory addon.<br />

In the Release directory the exe file, wanted directory and his dll file, the exe will not work without!<br /><br />
The Berkeley "Theory" used:<br />
<img src="https://github.com/RayColt/WebCam/blob/master/Resources/the-berkeley-theory.jpg" /><br />
First, grab image from web cam called Old. Second, grab another image from web cam after a while, called Cur. Compare Cur & Old by comparing each pixel color. 
If difference is greater than tested value, then save the two pictures.<br />
Finally, make the old picture the Cur picture. Back to second step (loop).
<br /><br />
<img src="https://github.com/RayColt/WebCam/blob/master/bin/Release/wanted/webcam.jpg" /><br />
