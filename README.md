<h1 align="center">USB Drive Data Stealer</h1>

<h2>Disclaimer &#9888;&#65039;</h2>
USB Drive Data Stealer is developed for educational purposes only.<br>
Responsibility for consequences of using this application remains with the user; <b>I'm not responsible for how you use it</b>.<br>

<h2>What is it for? &#129300;</h2>
It's a simple tool that steals a drive's data when it is connected to your pc.<br>
It's currently programmed to steal <b>removable drive</b> (USB flash drives, external hard disks, ...) or <b>CD\DVD</b> data only, but can be easily modified to steal data from <a href="https://learn.microsoft.com/en-us/dotnet/api/system.io.drivetype?view=net-8.0#fields" target="_blank" rel="noopener noreferrer">these</a> drives as well, by simply modifying the if clause <code>if (newDrive.DriveType == DriveType.Removable || ...)</code> of the <code>TMRwaitForDrive_Tick(...)</code> function.

<h2>Three different modes of operation &#49;&#65039;&#8419; &#50;&#65039;&#8419; &#51;&#65039;&#8419;</h2>
When someone plugs a USB drive into our PC, we don't know how long they will leave it plugged in.<br>
Suppose, for example, someone asks you to copy some files to their USB drive. Probably the copying of the files will not last very long.... Will USB Drive Data Stealer be able to steal all the contents of the USB drive in time?<br>
<br>
To solve this possible problem I implemented three different modes of operation:
<ol>
  <li>
    <b>Steal all device files</b><br>
    The entire contents of the drive will be copied to your PC.<br>
    If it contains large files and the data transfer rate is low, the probability of not being able to copy all the data before the drive is unplugged increases.
  </li>
  
  <li>
    <b>Steal all files smaller or equal than a specified size</b><br>
    Only files smaller or equal than the size you specified (in megabytes) via the GUI will be copied to your PC.<br>
    This solves the problem explained in the previous point discarding the larger files and, consequently, decreasing the time needed to copy the others.
  </li>
  
  <li>
    <b>Steal only files with specific extensions</b><br>
    Only files with an extension among those you have listed via the GUI will be copied to your PC.<br>
    If you are interested in copying only some particular types of files (e.g. PDF, images, audio, ...), this mode of operation is for you.<br>
    Since all types of files with extensions other than those allowed will not be copied, even in this case, the time required for copying will be shorter.
  </li>
</ol>

<h2>Demo &#127910;</h2>
This demonstration shows the execution of USB Drive Data Stealer, set to run in the first mode of operation (&quot;Steal all device files&quot;).<br>
<br>
<p align="center">
  <img src="DemoAllFiles.gif" title="IGPF is downloading the barol92 JSON files">
</p>

<h2>How it works? &#9881;&#65039;</h2>
Once the Instagram username and the keyword have been entered, IGPF will list all the links to the user's posts whose caption contains the keyword.<br>
<br>
About the keyword:
<ul>
  <li>
    If the keyword is contained in another word (e.g. keyword = <b>example</b> and word = counter<b>example</b>s), it will still be recognized.
  </li>
  
  <li>
    Keyword searching is case insensitive (e.g. keyword = hello and word = HeLLo is equivalent to keyword = hello and word = hello).
  </li>
  
  <li>
    The keyword can contain any symbol (therefore, for example, it can also be an hashtag), with the exception of spaces (otherwise you would be entering several keywords and not just one).
  </li>
</ul>

If matches are found (listed as Instagram image <a href="https://elfsight.com/blog/2015/10/how-to-get-instagram-photo-shortcode/" target="_blank" rel="noopener noreferrer">shortcodes</a>), just tap on one to view the matching post and its caption. The post will be shown through your device’s default browser (or through the Instagram app (if it's installed)).

<h2>What do I need to execute USB Drive Data Stealer? &#9654;</h2>
You'll need two things:
<ol>
  <li>
    The .NET Framework<br>
    I wrote USB Drive Data Stealer in C# for .NET Frameowrk 4.5 using Visual Studio 2019 so, in order to run it, you must have that version of the framework (or a later one) installed.<br>
    If it isn't already installed on your OS, you can download it from <a href="https://www.microsoft.com/en-us/download/details.aspx?id=30653" target="_blank" rel="noopener noreferrer">here</a>.
  </li>

  <li>
    USBDriveDataStealer.exe<br>
    You can download it directly from <a href="https://github.com/LucaBarile/USBDriveDataStealer/raw/main/USBDriveDataStealer.exe" target="_blank" rel="noopener noreferrer">here</a>.<br>
    If you want to compile it or modify its source code, you can download the zipped project <a href="https://github.com/LucaBarile/USBDriveDataStealer/raw/main/USBDriveDataStealer.zip" target="_blank" rel="noopener noreferrer">here</a> and recompile it.
  </li>
</ol>

<h2>Aims of the program &#127919;</h2>
<ul>
  <li>
    Make yourself aware that by plugging an USB drive into a device, you are always exposing its data to a potential risk.<br>
    This is true even if the device belongs to a person you know and trust; it may have been infected by a malware that, among other malicious actions, could do exactly what USB Drive Data Stealer does, sending the collected data to the attacker.<br>
    You could solve the problem by using a USB drive containing nonconfidential files whose theft and/or publication would not be a problem for you.
  </li>
  
  <li>
    Show, by means of an engaging program (imo), the basics of C# and event-driven programming, and the ease with which a Windows GUI application can be built using this language.
  </li>

  <li>
    Show, for teaching purposes, an example of simple malware.
  </li>
  
  <li>
    Talking about cybersecurity.
  </li>
</ul>

<h2>Download links &#128229;</h2>
<a href="https://github.com/LucaBarile/USBDriveDataStealer/raw/main/USBDriveDataStealer.exe" target="_blank" rel="noopener noreferrer">Here</a> you can download USBDriveDataStealer.exe<br>
<a href="https://github.com/LucaBarile/USBDriveDataStealer/raw/main/USBDriveDataStealer.zip" target="_blank" rel="noopener noreferrer">Here</a> you can download the Visual Studio 2019 zipped project.<br>
<br>
<hr>
<a href="https://lucabarile.github.io/" target="_blank">Here</a> you can visit my website &#127760;<br>
<a href="https://www.buymeacoffee.com/LucaBarile" target="_blank">Here</a> you can buy me a unicorn &#129412;
<hr>
