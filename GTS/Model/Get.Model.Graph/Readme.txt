http://developers-blog.org/blog/default/2009/05/23/JUNG-Graphen-modellieren-mit-Java 

http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part3.aspx


http://social.msdn.microsoft.com/Forums/en-US/wcf/thread/f4e8edd0-4e20-49b7-ab69-9eb671c36c16/#037a71b1-1ad8-4183-975f-32d8315c335b
diskussion ob man guid ins model tun soll oder nicht

hallo, 
27.03.2012  14:37:53  .uservorname  MSDN Messenger Hotline  ist es normal, dass die methode internal void OnSerialized(StreamingContext context) context.contex=null ist wenn man debugt? 
27.03.2012  14:38:01  MSDN Messenger Hotline  .uservorname  Hallo und willkommen bei der MSDN Hotline!  
27.03.2012  14:39:01  MSDN Messenger Hotline  .uservorname  Einen Augenblick, dass muss ich mir genauer anschauen 
27.03.2012  14:43:06  MSDN Messenger Hotline  .uservorname  meinen Sie die Methode, die im Beispiel unten aufgeführt ist? http://msdn.microsoft.com/de-de/library/system.runtime.serialization.onserializedattribute.aspx#Y1680 
27.03.2012  14:43:21  .uservorname  MSDN Messenger Hotline  genau 
27.03.2012  14:45:02  MSDN Messenger Hotline  .uservorname  Zitat: "Um OnSerializedAttribute verwenden zu können, muss die Methode einen StreamingContext-Parameter enthalten". das weist darauf hin, dass der StreamingContext wohl nicht in jedem Fall gesetzt werden muss 
27.03.2012  14:45:43  .uservorname  MSDN Messenger Hotline  was kann man genau mit dem StreamingContext-Parameter machen? 
27.03.2012  14:47:53  MSDN Messenger Hotline  .uservorname  Hier nochmal zu Ihrer Fragestellung: http://msdn.microsoft.com/de-de/library/system.runtime.serialization.streamingcontext.context.aspx . Zitat:" Dieser kann null sein. " 
27.03.2012  14:48:29  .uservorname  MSDN Messenger Hotline  ah ok. 
27.03.2012  14:50:20  MSDN Messenger Hotline  .uservorname  Also der StreamingContext kann wohl je nach Anwendung mit weiteren Informationen versehen werden 
27.03.2012  14:51:23  MSDN Messenger Hotline  .uservorname  Beispielsweise einen Zeitstempel oder ähnliches 
27.03.2012  14:52:48  .uservorname  MSDN Messenger Hotline  ah ok 
27.03.2012  14:54:56  .uservorname  MSDN Messenger Hotline  es ist also nicht möglich über die OnSerialized Methode einen weiteren zusätzlichen wert zur serialization zu speichern? 
27.03.2012  14:56:13  .uservorname  MSDN Messenger Hotline  also wenn ich eine klasse habe: public class { [DataMember(Name = "U", Order = 2, IsRequired = true)] propertie ...} und ich zur serialisieren klasse einen zusätzliche information speichern will  
27.03.2012  14:57:28  MSDN Messenger Hotline  .uservorname  Für mich sieht es schon so aus, dass man dort auch weitere Informationen gespeichert werden kann 
27.03.2012  14:57:32  MSDN Messenger Hotline  .uservorname  hier: http://msdn.microsoft.com/de-de/library/system.runtime.serialization.streamingcontext.context.aspx 
27.03.2012  14:57:46  MSDN Messenger Hotline  .uservorname  es wird beim Auslesen entsprechend interpretiert 
27.03.2012  14:57:59  MSDN Messenger Hotline  .uservorname  in diesem Fall als DateTime 
27.03.2012  14:59:15  .uservorname  MSDN Messenger Hotline  das ISerialize interface funktioniert mit dem DataContract Attribut? 
27.03.2012  14:59:29  .uservorname  MSDN Messenger Hotline  bzw. wenn ich es über NEtDataContract serialisiere? 
27.03.2012  15:18:45  MSDN Messenger Hotline  .uservorname  Bitte entschuldigen Sie die Wartezeit 
27.03.2012  15:19:05  MSDN Messenger Hotline  .uservorname  meinen DataContract oder Context? 
27.03.2012  15:19:49  MSDN Messenger Hotline  .uservorname  The serializer can serialize types to which either the DataContractAttribute or SerializableAttribute attribute has been applied 
27.03.2012  15:20:23  MSDN Messenger Hotline  .uservorname  aus http://msdn.microsoft.com/en-us/library/system.runtime.serialization.netdatacontractserializer.aspx 
27.03.2012  15:34:39  .uservorname  MSDN Messenger Hotline  DataContract  
27.03.2012  15:35:05  .uservorname  MSDN Messenger Hotline  aha dann müsste das eh funktionieren wenn ich das interface implementiere und Datacontract verwende 
