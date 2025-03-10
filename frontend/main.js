cwindow.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
});

const functionApi = "YOUR_API_ENDPOINT_HERE"; // Replace with the actual API endpoint

const getVisitCount = () => {
    let count = 30;
    
    fetch(functionApi)
        .then(response => response.json())
        .then(response => {
            console.log("Website called function API.");
            count = response.count;
            document.getElementById("counter").innerText = count;
        })
        .catch(function (error) {
            console.log(error);
            return count;
        });
};
