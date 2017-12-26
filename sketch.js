var apiKey = "a5684ff069c74930beff5c4ccccbaa06"
var url = "https://newsapi.org/v1/articles?source=bbc-news&sortBy=top&apiKey="
var images = []
var loadedImages = []

function setup() {
	loadJSON(url + apiKey, gotData)
}

function gotData(data){
	console.log(data)
	for (var i = 0; i < data.articles.length; i++){
		console.log(data.articles[i].title)
		images.push(data.articles[i].urlToImage)
	}
	for (var i = 0; i < images.length; i++){
		loadImage(images[i])
	}
}

function imageLoaded(image){
	loadedImages.push(image)
}

function draw() {
	if (loadedImages.length == images.length){
		var x = 0
		var y = 0
		for (var i = 0; i < loadedImages.length; i++){
			image(loadedImages[i], x, y)
			x += loadedImages[i].width + 5
			if (x > width){
				x = 0
				y += 200
			}
		}
	}
}