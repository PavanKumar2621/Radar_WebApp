
const RadarDataManager = {
    tracks: {},
    TRACK_TIMEOUT: 3000,
    MAX_HISTORY: 500,
    handlers: [],
    removeHandlers: [],
    
    // Register handler for new/update radar data
    addHandler(handler) {
        this.handlers.push(handler);
    },

    // Register handler for track removal
    addRemoveHandler(handler) {
        this.removeHandlers.push(handler);
    },

    // PROCESS RADAR DATA
    process(message) {
        if (!message || !message.data) {
            console.error("Invalid radar message");
            return;
        }

        const data = message.data;
        const trackId = data.trackId;
        if (trackId == null) {
            console.error("Radar data has no Track ID");
            return;
        }

        // CREATE TRACK IF IT DOES NOT EXIST
        if (!this.tracks[trackId]) {
            this.tracks[trackId] = {
                data: data,
                lastSeen: Date.now(),
                history: {
                    time: [],
                    azimuth: [],
                    elevation: [],
                    range: [],
                    velocity: [],
                    snr: []
                }
            };
            console.log(`Created Track ${trackId}`);
        }

        // GET EXISTING TRACK
        const track = this.tracks[trackId];

        // UPDATE CURRENT DATA
        track.data = data;
        track.lastSeen = Date.now();

        // STORE HISTORY
        const timestamp = Date.now();
        track.history.time.push(timestamp);


        // Azimuth
        if (data.azimuth != null) {
            track.history.azimuth.push(data.azimuth);
        }
        else {
            track.history.azimuth.push(null);
        }

        // Elevation
        if (data.elevation != null) {
            track.history.elevation.push(data.elevation);
        }
        else {
            track.history.elevation.push(null);
        }

        // Range
        if (data.range != null) {
            track.history.range.push(data.range);
        }
        else {
            track.history.range.push(null);
        }

        // Velocity
        if (data.velocity != null) {
            track.history.velocity.push(data.velocity);
        }
        else {
            track.history.velocity.push(null);
        }

        // SNR
        if (data.snr != null) {
            track.history.snr.push(data.snr);
        }
        else {
            track.history.snr.push(null);
        }

        // LIMIT HISTORY SIZE
        if (track.history.time.length > this.MAX_HISTORY) {
            track.history.time.shift();
            track.history.azimuth.shift();
            track.history.elevation.shift();
            track.history.range.shift();
            track.history.velocity.shift();
            track.history.snr.shift();
        }

        // NOTIFY DASHBOARD / COMPONENTS
        this.handlers.forEach(handler => {
            try {
                handler(data);
            }
            catch (error) {
                console.error("Radar handler error:", error);
            }
        });
    },

    // GET SINGLE TRACK
    getTrack(trackId) {
        return this.tracks[trackId] ?? null;
    },

    // GET ALL TRACKS
    getAllTracks() {
        return this.tracks;
    },

    // GET HISTORY
    getTrackHistory(trackId) {
        const track = this.tracks[trackId];
        if (!track) {
            return null;
        }
        return track.history;
    }
};

// Receive radar data from SignalR
connection.on(
    "ReceiveRadarData",
    function (message) {
        RadarDataManager.process(message);
    }
);

// Remove tracks that have stopped sending data
setInterval(function () {
    const now = Date.now();
 
    for (const trackId in RadarDataManager.tracks) {
        const track = RadarDataManager.tracks[trackId];
        if (now - track.lastSeen > RadarDataManager.TRACK_TIMEOUT) {
 
            // Remove from manager
            delete RadarDataManager.tracks[trackId];
            console.log(`Track ${trackId} removed`);

            // Notify Dashboard/pages
            RadarDataManager.removeHandlers.forEach(
                handler => {
                    try {
                        handler(trackId);
                    }
                    catch (error) {
                        console.error("Radar remove handler error:", error);
                    }
                }
            );
        }
    }
}, 1000);

