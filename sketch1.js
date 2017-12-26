var recog = new p5.SpeechRec()
var speech = new p5.Speech()
var url = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20171226T191934Z.9c6f27c98a3eb908.926a60f7b2ddd3737537ea4df958acd36dcabaf0&"
var apiKey = "trnsl.1.1.20171226T191934Z.9c6f27c98a3eb908.926a60f7b2ddd3737537ea4df958acd36dcabaf0"

function setup() {
	createCanvas(windowWidth, windowHeight)
	textAlign(CENTER)
	textSize(32)
	speech.setVolume(0.1)
	speech.setLang("fr")
	recog.onResult = showResult
	recog.start()
}

function draw() {

}

function showResult(){
	text(recog.resultString, width/2, height/2)
	loadJSON(url + apiKey + "&text=" + recog.resultString + "&lang=fr-en", gotData)
	//speech.speak(recog.resultString)
}

function gotData(data){
	text(data.text, width/2, height/2 + 50)
	speech.speak(data.text)
}

function keyPressed(){
	background(255)
	recog.onResult = showResult
	recog.start()
}