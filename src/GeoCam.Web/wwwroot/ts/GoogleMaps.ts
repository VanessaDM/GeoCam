/// <reference path="./types/googlemaps/index.d.ts" />

let googleMap: GoogleMap;

interface CameraModel {
    name: string;
    longitude: number;
    latitude: number;
}

class GoogleMap {
    initBlazor = false; // True if we received an init call from Blazor
    initScript = false; // True if the Google Maps script has loaded
    // *TODO: should not be hardcoded
    defaultMapCenter: google.maps.LatLng = new google.maps.LatLng(52.094987, 5.078397);
    defaultMapZoom = 12;
    cameras: CameraModel[] = null;
    map: google.maps.Map = null;
    infoWindow: google.maps.InfoWindow = null;

    constructor() {
    }

    public init() {
        if (!this.initBlazor || !this.initScript || this.map) {
            return;
        }

        this.map = new google.maps.Map(document.getElementById('map'), { center: this.defaultMapCenter, zoom: this.defaultMapZoom });
        this.infoWindow = new google.maps.InfoWindow();
        if (this.cameras) {
            this.addMarkers(this.cameras);
            this.cameras = null;
        }
    }

    public addMarkers(cameras: CameraModel[]) {
        // If we get the markers before the map has loaded then store them for later
        if (!this.map) {
            this.cameras = cameras;
            return;
        }

        for (let idx = 0; idx < cameras.length; idx++) {
            const camera = cameras[idx];

            const mapMarker = new google.maps.Marker({
                position: { lat: camera.latitude, lng: camera.longitude },
                map: this.map,
                title: camera.name,
            });

            google.maps.event.addListener(mapMarker, 'click', () => {
                this.infoWindow.setContent(`Camera: ${camera.name}<br />${camera.latitude} / ${camera.longitude}`);
                this.infoWindow.open(this.map, mapMarker);
            });
        }
    }
}

(<any>window).initGoogleMapBlazor = () => {
    googleMap = googleMap || new GoogleMap();
    googleMap.initBlazor = true;
    googleMap.init();
};

(<any>window).initGoogleMapScript = () => {
    googleMap = googleMap || new GoogleMap();
    googleMap.initScript = true;
    googleMap.init();
};
