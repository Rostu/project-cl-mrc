/** @mainpage JaDe-Tokenizer 
 *
 *  @section sec2 Projekt-Zielsetzung
 *  
 *  Im Rahmen eines ein-semestrigen Projektseminars war es unser Ziel eine Windows-Forms Anwendung in c# zu erstellen.
 *  Grundidee war es ein Tool bereit zu stellen mit dem man japanische Texte in Token zerlegen kann. 
 In der Benutzeroberfläche sollte es die Möglichkeit geben fehlerhaft erstellte Token zu bearbeiten. Außerdem sollte es möglich sein die Bedeutung der Token in einem eingebundenen Wörterbuch(Jap-Deu) nachzuschlagen.
 Soweit die Funktionen welche von uns auch Umgesetzt wurden. 
 *  @subsection Projekt-Mitglieder
 -  Christopher Michels
 -  Mirko Daumann
 -  Robert Studtrucker
 -  Youness El Ouafy

 @section sec3 Ausblick
 Im weiteren möchten zumindest Teile der Projektgruppe an dem Projekt weiter arbeiten. Daher hier noch ein Paar Funktionen/Ziele die noch nicht Umgesetzt wurden aber folgen werden:
 -  Ausgabe des japanischen Textes mit Übersetzungesvorschlägen für einzelne Token in einer formatierten Datei (xml).
  -  Möglichkeit die gewählte Übersetzung für einen Token zu ändern.
  -  Die Möglichkeit während der Bearbeitung der Token den Fortschritt zu speichern.
 -  Zusätzliches anlegen eines Nachschlagewerk für Japanische Postpositionen (mit grammatikalisch hilfreichen Einträgen).  
 -	Auswahl der Token zum Bearbeiten und Suchen überarbeiten.
 -  Funktion des Tokenizers überarbeiten.

 *  @section sec4 Verwendete Resourcen
 *  @subsection TinySegmenter
 *  <p>Da dies unser erstes Programmier-Projekt war  und wir mit allen Möglichen Komplikationen rechneten haben wir uns aus zeitlichen Gründen dazu entschlossen keinen eigenen Tokenizer für das Japanische zu trainieren. 
 *  Statt dessen haben wir auf einen bereits implementierten Tokenizer namens TinySegmenter zurückgegriffen. 
 *  Diesen haben wir lediglich in c# umgesetzt und unseren Bedürfnissen Entsprechend angepasst.</p>
 *  <p>Der TinySegmenter ist ein durch einen maschinellen Lernprozess erstelltes Modell zur Erkennung von Wortgrenzen im Japanischen, er verwendet kein Wörterbuch, arbeitet also nach dem Prinzip der Word-Boundary-Prediction.
 *  Im maschinellen Lernprozess wurde der RWCP-Corpus verwendet. Daher ist der TinySegmenter stark bei Zeitungstexten, hat aber Schwächen bei „einfachen“ Texten, die in Hiragana verfasst sind.</p>
 *  Der TinySegmenter ist frei verfügbar unter: 
 *  <p><a href="http://chasen.org/~taku/software/TinySegmenter/">http://chasen.org/~taku/software/TinySegmenter/</a></p>
 *  <p><a href="http://chasen.org/~taku/software/TinySegmenter/tiny_segmenter-0.1.js">http://chasen.org/~taku/software/TinySegmenter/tiny_segmenter-0.1.js</a></p>
 *  <p>Die copyright notice findet sich in der Segmenter Klasse welche maßgeblich auf dem TinySegmenter aufbaut. </p>
 *  @subsection WaDoku
 *	<p>Ebenfalls frei verfügbar ist das von uns verwendete Wörterbuch, eine xml Version des Wörterbuchprojektes von Ulrich Apel.</p>
 * 	<p><a href="http://www.wadoku.de/wiki/x/ZQE">http://www.wadoku.de/wiki/x/ZQE</a></p>
 *	<p>Die copyright notice findet sich in der SearchEngine Klasse.</p>
 *  @subsection Sonstiges
 *  <p>Microsoft Visual Studio Professional 2010</p>
 *  <p>DoxyGen</p>
 *  <p><a href="http://www.stack.nl/~dimitri/doxygen/index.html">http://www.stack.nl/~dimitri/doxygen/index.html</a></p>
 */