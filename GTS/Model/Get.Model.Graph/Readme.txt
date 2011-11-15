http://developers-blog.org/blog/default/2009/05/23/JUNG-Graphen-modellieren-mit-Java 

http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part3.aspx


MSDN Messenger Hotline sagt (14:24):
 Wenn Sie statt des XML-Serializer einen DataContractSerializer verwenden können Sie auch zirkuläre Referenzen serialisieren
 http://msdn.microsoft.com/de-de/library/system.runtime.serialization.datacontractserializer.aspx
 http://msdn.microsoft.com/de-de/library/system.runtime.serialization.datacontractserializer.preserveobjectreferences.aspx
.uservorname sagt (14:25):
 ah ausgezeichnet
.uservorname sagt (14:31):
 die graph controls in denen das Graph Model visualisiert (mit wpf) wird, muss auch gespeichert werden... kann ich dafür auch den datacontractserializer verwenden?
MSDN Messenger Hotline sagt (14:35):
 Solange die Objekte serialisierbar sind sollte das funktionieren
MSDN Messenger Hotline sagt (14:36):
 Oder wollen Sie das Bild speichern von dem Graph?
.uservorname sagt (14:38):
 nein die positionen z.b
.uservorname sagt (14:39):
 wenn ich nur das bild speichere, kann ich beim nächsten start den graph nicht wiederherstellen
 ich hab irgendwo gelesen, dass die observablecollection beim serialisieren probleme macht
 vlt wäre es hier wirklich einfacher, eine eigene xml datei für die controls zu erstellen?
MSDN Messenger Hotline sagt (14:41):
 Für die ObservableCollection habe ich diese Info gefunden: http://kentb.blogspot.com/2007/11/serializing-observablecollection.html
 Oder Sie erstellen eine eigene XML-Datei
MSDN Messenger Hotline sagt (14:42):
 Oder auch hier noch eine andere Lösung: http://www.tanguay.info/web/index.php?pg=codeExamples&id=224
.uservorname sagt (14:42):
 da ich beim serializieren der wpf controls u.a auch eine exception beim Typ Binding konnte nicht serialisiert wreden erhalte, wird wohl das erstellen der xml datei das beste sein 