import {
    HubConnectionBuilder,
    HttpTransportType,
    LogLevel,
} from "@aspnet/signalr";

export default {
    install(Vue) {
        const connection = new HubConnectionBuilder()
            .withUrl("http://192.168.0.16:5000/api/gameHub", {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets,
            })
            .configureLogging(LogLevel.Information)
            .build();

        const gameHub = new Vue();

        Vue.prototype.$gameHub = gameHub;

        //use of hub methods
        gameHub.joinGame = (lobbyid) => {
            return startedPromise
                .then(() => connection.invoke("JoinGame", lobbyid))
                .catch(console.error);
        };

        gameHub.playerReady = (lobbyid) => {
            return startedPromise
                .then(() => connection.invoke("Ready", lobbyid))
                .catch(console.error);
        };

        connection.on("Ready", (lobbyid) => {
            gameHub.$emit("lobby-ready", lobbyid);
        });

        gameHub.startGame = (lobbyid) => {
			
            return startedPromise
                .then(() => connection.invoke("Start", lobbyid))
                .catch(console.error);
        };

        connection.on("Start", (lobbyid) => {
            gameHub.$emit("start-game", lobbyid);
        });

        gameHub.shoot = (lobbyid, x, y, player) => {
			return startedPromise
                .then(() => connection.invoke("Shoot", lobbyid, x, y, player))
                .catch(console.error);
        };

        connection.on("Shoot", (lobbyid, x, y, player) => {
            gameHub.$emit("shoot-player", lobbyid, x, y, player);
        });
		
        gameHub.gridStatus = (lobbyid, x, y, status) => {
			return startedPromise
                .then(() => connection.invoke("GridStatus", lobbyid, x, y, status))
                .catch(console.error);
        };

        connection.on("GridStatus", (lobbyid, x, y, status) => {
            gameHub.$emit("send-status", lobbyid, x, y, status);
        });

        gameHub.victory = (lobbyid) => {
			return startedPromise
                .then(() => connection.invoke("Victory", lobbyid))
                .catch(console.error);
        };

        connection.on("Victory", (lobbyid) => {
            gameHub.$emit("get-user-win", lobbyid);
        });

        //try reconeccting if connection failed
        let startedPromise = null;
        function start() {
            startedPromise = connection.start().catch((err) => {
                console.error("Failed to connect with hub", err);
                return new Promise((resolve, reject) =>
                    setTimeout(() => start().then(resolve).catch(reject), 20000)
                );
            });
            return startedPromise;
        }
        connection.onclose(() => start());

        start();
    },
};
