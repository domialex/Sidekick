"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.HookService = void 0;
const connector_1 = require("./connector");
class HookService extends connector_1.Connector {
    constructor(socket, app) {
        super(socket, app);
        this.app = app;
    }
    onHostReady() {
    }
}
exports.HookService = HookService;
//# sourceMappingURL=index.js.map