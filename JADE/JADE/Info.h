/** @mainpage JaDe-Tokenizer 
 * <a href="index2.html" target="_parent"><b>Bedienungsanleitung</b></a>
 *  <br/>
 *  @section sec2 Projekt-Zielsetzung
 *  
 *  Im Rahmen eines einsemestrigen Projektseminars war es unser Ziel eine Windows-Forms Anwendung in C# zu erstellen.
 *  Grundidee war es, ein Tool bereitzustellen, mit dem man japanische Texte in Token zerlegen kann. 
 In der Benutzeroberfläche sollte es die Möglichkeit geben, fehlerhaft erstellte Token zu bearbeiten. Außerdem sollte es möglich sein die Bedeutung, der Token in einem eingebundenen Wörterbuch (Japanisch-Deutsch) nachzuschlagen.
 *  @subsection Projekt-Mitglieder
 -  Christopher Michels
 -  Mirko Daumann
 -  Robert Studtrucker
 -  Youness El Ouafy

 @section sec3 Ausblick
 Im Weiteren möchten zumindest Einzelne der Projektgruppe an dem Projekt weiter arbeiten. Daher hier noch ein paar Funktionen/Ziele die noch nicht umgesetzt wurden aber folgen werden:
 -  Ausgabe des japanischen Textes mit Übersetzungsvorschlägen für einzelne Token in einer passend formatierten Datei (Xml)
  -  Möglichkeit, die gewählte Übersetzung für einen Token zu ändern
  -  Möglichkeit, während der Bearbeitung der Token den Fortschritt zu speichern
 -  Zusätzliches anlegen eines Nachschlagewerkes für japanische Postpositionen (mit grammatikalisch hilfreichen Einträgen) 
 -	Auswahl der Token zum Bearbeiten und Suchen überarbeiten
 -  Funktion des Tokenizers überarbeiten

 *  @section sec4 Verwendete Resourcen
 *  @subsection TinySegmenter
 *  <p>Da dies unser erstes Programmier-Projekt war und wir mit verschiedenen Komplikationen rechneten, haben wir uns aus zeitlichen Gründen dazu entschlossen, keinen eigenen Tokenizer für das Japanische zu trainieren. 
 *  Stattdessen haben wir auf einen bereits implementierten Tokenizer namens TinySegmenter zurückgegriffen. 
 *  Diesen haben wir lediglich in C# umgesetzt und unseren Bedürfnissen entsprechend angepasst.</p>
 *  <p>Der TinySegmenter ist ein durch einen maschinellen Lernprozess erstelltes Modell zur Erkennung von Wortgrenzen im Japanischen. Er verwendet kein Wörterbuch, arbeitet also nach dem Prinzip der Word-Boundary-Prediction.
 *  Im maschinellen Lernprozess wurde der RWCP-Corpus verwendet. Daher ist der TinySegmenter stark bei Zeitungstexten, hat aber Schwächen bei „einfachen“ Texten, die in Hiragana verfasst sind.</p>
 *  Der TinySegmenter ist frei verfügbar unter: 
 *  <p><a href="http://chasen.org/~taku/software/TinySegmenter/">http://chasen.org/~taku/software/TinySegmenter/</a></p>
 *  <p><a href="http://chasen.org/~taku/software/TinySegmenter/tiny_segmenter-0.1.js">http://chasen.org/~taku/software/TinySegmenter/tiny_segmenter-0.1.js</a></p>
 *  <p>Der Urheberrechtsvermerk befindet sich in der Segmenter-Klasse, welche maßgeblich auf dem TinySegmenter aufbaut. </p>
 *  @subsection WaDoku
 *	<p>Ebenfalls frei verfügbar ist das von uns verwendete Wörterbuch, eine xml Version des Wörterbuchprojektes von Ulrich Apel.</p>
 * 	<p><a href="http://www.wadoku.de/wiki/x/ZQE">http://www.wadoku.de/wiki/x/ZQE</a></p>
 *	<p>Der Urheberrechtsvermerk findet sich in der SearchEngine-Klasse.</p>
 *  @subsection Sonstiges
 *  <p>Microsoft Visual Studio Professional 2010</p>
 *  <p>AnkSVN</p>
 *  <p><a href="">http://www.stack.nl/~dimitri/doxygen/index.html</a></p>
 *  <p>Google Code</p>
 *  <p><a href="">http:/</a></p>
 *  <p>DoxyGen</p>
 *  <p><a href="http://www.stack.nl/~dimitri/doxygen/index.html">http://www.stack.nl/~dimitri/doxygen/index.html</a></p>
 *  
 *  
 */