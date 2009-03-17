// load OSM map on element 'id' and center on provided location
function loadMap(id, lat, lon) {
    var zoom = 10

    var map; //complex object of type OpenLayers.Map

    map = new OpenLayers.Map(id, {
        controls: [
                    new OpenLayers.Control.Navigation(),
                    new OpenLayers.Control.PanZoomBar(),
                    new OpenLayers.Control.Attribution()],
        maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
        maxResolution: 156543.0399,
        numZoomLevels: 19,
        units: 'm',
        projection: new OpenLayers.Projection("EPSG:900913"),
        displayProjection: new OpenLayers.Projection("EPSG:4326")
    });


    // Define the map layer
    // Note that we use a predefined layer that will be
    // kept up to date with URL changes
    // Here we define just one layer, but providing a choice
    // of several layers is also quite simple
    // Other defined layers are OpenLayers.Layer.OSM.Mapnik, OpenLayers.Layer.OSM.Maplint and OpenLayers.Layer.OSM.CycleMap
    layerTilesAtHome = new OpenLayers.Layer.OSM.Mapnik("Mapnik");
    map.addLayer(layerTilesAtHome);

    var lonLat = new OpenLayers.LonLat(lon, lat).transform(new OpenLayers.Projection("EPSG:4326"), map.getProjectionObject());

    map.setCenter(lonLat, zoom);
}