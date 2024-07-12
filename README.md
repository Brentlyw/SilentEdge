# SilentEdge
A silent way to launch edge with an attachable socket, for stealthy C2 communications.


Something to note:

Msedge.exe will not open a \Device\Afd handle (for sockets) until a website is visited. While spawning the process detatched and hidden, appending the argument "www.google.com" which would normally direct the browser to Google, does not actually visit the website and still will not open a \Device\Afd handle. But edge is Chromium based, which allows us to pass --remote-debugging-port=9222 as an argument which will open a \Device\Afd handle on edge, even if hidden.

The above is not included in the repository code, but is easily implemented as an argument.
