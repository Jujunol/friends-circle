function initMap() {

    var input = /** @type {!HTMLInputElement} */(
        document.getElementById('FromWhereCity')
        || 
        document.getElementById('friend_street'));

    var autocomplete = new google.maps.places.Autocomplete(input);

    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            window.alert("Autocomplete's returned place contains no geometry");
            return;
        }
    });

}