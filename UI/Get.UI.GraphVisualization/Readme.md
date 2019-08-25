
1. https://github.com/AvaloniaUI/avalonia-dotnet-templates
2. dotnet new avalonia.app -o DataStructures.UI.Demo  
3. Remove old System.Windows namespace and add where possible the new Avalon. 
4. Fix simple stuff like Size.Width+=10 doesn't work as its readonly with new Size(size.Width +10)
5. replace FrameworkPropertyMetadata with StyledPropertyMetadata
6. Rest auskommentieren



Gut zu wissen:
Die Edge Controls werden ohne Positionsangabe zum Graph Control hinzugefügt! Ledeglich die Line hat die richtigen Koordinaten, d.h. das Edge Control kann an einem völlig anderem Ort liegen
obwohl die Line korrekt angezeigt wird.


Probleme:
Scrollviewer
Wird ein Scrollviewer zum Graph hinzugefügt werden die Adornar Elemente zu den Vertex Controls nicht mehr hinzugefügt oder angezeigt.
Die Edge Controls verlieren sofort den Focus nach dem Aufruf von this.Focus() [1]
[1] http://social.msdn.microsoft.com/Forums/vstudio/en-US/7c44fb63-46a9-4a71-beda-79cd7fdb08c4/shapes-are-not-getting-focus

Todos:
Edges an richtige position setzen und dann Grid und TextBox verwenden.
Edge von panel ableiten
2x DP U und V machen die vom typ VertexVisualzion sind. positionen sollen von diesen dps geholt werden

Threads:
http://social.msdn.microsoft.com/Forums/vstudio/en-US/955593ed-cdae-438e-8c1a-d63d851aa197/make-invisible-area-clickable-in-custom-control