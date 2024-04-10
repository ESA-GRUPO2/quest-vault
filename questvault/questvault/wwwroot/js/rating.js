const stars = document.querySelectorAll(".star");
const rating = document.getElementById("rating");
const ratingV = document.getElementById("ratingV");
const reviewText = document.getElementById("review");
const submitBtn = document.getElementById("submit");
const reviewsContainer = document.getElementById("reviews");

stars.forEach((star) => {
	star.addEventListener("click", () => {
		const value = parseInt(star.getAttribute("data-value"));
		rating.innerText = value;
		ratingV.value = value;

		
		// Remove all existing classes from stars
		stars.forEach((s) => s.classList.remove("one",
			"two",
			"three",
			"four",
			"five"));

		// Add the appropriate class to 
		// each star based on the selected star's value
		stars.forEach((s, index) => {
			if (index < value) {
				s.classList.add(getStarColorClass(value));
			}
		});
		
		// Remove "selected" class from all stars
		stars.forEach((s) => s.classList.remove("selected"));
		// Add "selected" class to the clicked star
		star.classList.add("selected");
	});
});

submitBtn.addEventListener("click", () => {
	const review = reviewText.value;
	const userRating = parseInt(rating.innerText);

	if (!userRating || !review) {
		alert(
			"Please select a rating and provide a review before submitting."
		);
		return;
	}

	if (userRating > 0) {
		const reviewElement = document.createElement("div");
		ratingValue = userRating;
		reviewValue = review;
		reviewElement.classList.add("review");
		reviewElement.innerHTML =
			`<p><strong>Rating: ${userRating}/5</strong></p><p>${review}</p>`;
		reviewsContainer.appendChild(reviewElement);

		// Reset styles after submitting
		reviewText.value = "";
		rating.innerText = "0";
		stars.forEach((s) => s.classList.remove("one",
			"two",
			"three",
			"four",
			"five",
			"selected"));
	}
});

function getStarColorClass(value) {
	switch (value) {
		case 1:
			return "one";
		case 2:
			return "two";
		case 3:
			return "three";
		case 4:
			return "four";
		case 5:
			return "five";
		default:
			return "";
	}
}