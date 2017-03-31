# NTRSharp  
Library for C# Applications that use NTR made by cell9

---

## What is this?
This is the successor to the [NtrClient](https://github.com/imthe666st/NTRClient) written by me a few months ago. It's no longer supported and was very buggy. Thus I started working on a complete rewrite of the entire project starting with rewriting the NTRClient and adding/removing stuff.  
**This project is not yet finished. Use with caution.**

### Can you contribute to the project?

Yes.  
If you have any [Issues](https://github.com/imthe666st/NTRSharp/issues) or questions feel free to commentate on an already existing Issue or open a new one.  
If you have any ideas for the debugger feel free to open an issue as well. I'll try my best to review your ideas. Alternatively you can also fork the project and start implementing your ideas. Once you're done just send a Merge request and I'll have a look at it.

---

## What is a Base64 Code?
[Base64](https://en.wikipedia.org/wiki/Base64) is a group of similar binary-to-text encoding schemes that represent binary data in an ASCII string format by translating it into a radix-64 representation. This means we can easily create a (somewhat) human readable code that can easily be exchanged with your friends to send them items, equipment or similar.  

### How to create Base64 Codes
There are two methods to create Base64 Codes.  
**Easy method**:  
> Open the debugger and connect to your 3ds. Select the process you want to create a code for. Once the Memregions appeared go to "Base64 Codes" and enter the address and length of the code. Once that's done just click "Create Code" and the Base64 code should appear. You can now exchange this code with your friends and give them items, equipment or similar. 

**Advanced method**:  
> Open the debugger and go to "Code Editor". There you can start creating a new code by entering an address and process name. Now all that's left is the data. The left textbox functions as a hexeditor where you can start writing bytes. If you want to export your code as Base64 simply encrypt it and copy from the right textbox. If you have a code you want to modify simply paste a Base64 code in the right textbox and click decrypt it. Now you can edit it in the hexeditor. You can additionally load a file.

### How to use Base64 Codes
If you received a Base64 code open the debugger and connect to your 3ds. Select the process this code is for. Now go to "Base64 Codes" and paste the code into the textbox. All that's left is hitting the "Use Code" button which will do the rest. 
