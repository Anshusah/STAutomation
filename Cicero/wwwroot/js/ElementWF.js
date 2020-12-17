var stage = new Vesuvio3D({ width: 1400, height: 800, fill: "#ff0000", id: "workflow-vp" });
class WF {
    constructor(e) {
        this.show = (e && typeof (e.show) !== 'undefined') ? e.show : true;
        this.inPath = false;
        this.countDown = 0;
        this.oldCursor = null;
        this.mapImage = null;
        this.pointers = [];
        this.objects = [];
        this.id = e.id;
        this.vars = {
            formId: null,
            deleteStateForForm: "",
            addStateForForm: "",
            elementId: "",
            eventType:""

        };
        //this.vars = {};
        this.selected = null;
        this.globalUnlockPoints = false;
        this.tools = {
            delete: false,
            save: false,
        };
        stage.mouse.onDown(function () {

            var self = this;
            stage.mouse.lastX = stage.mouse.down.stageX;
            stage.mouse.lastY = stage.mouse.down.stageY;
            this.inActivePoints();
            this.inActiveObjects();
            if (this.isPointerInPath() === false && this.isPointerInObjectPath() === false) {
                var point = self.getPointer(stage);
                point.active = true;
                point.points[1].lock = false;
                point.points[0].lock = true;
                this.pointers.push(point);
            }

        }.bind(this));
        stage.mouse.onUp(function () {
            var self = this;
            stage.mouse.lastX = stage.mouse.down.stageX;
            stage.mouse.lastY = stage.mouse.down.stageY;
            if (this.pointers.length) {
                var toRemove = [];
                this.pointers.forEach(function (point, index) {
                    point.active = false;
                    point.points[0].lock = true;
                    point.points[1].lock = true;
                    point.inPath = false;

                    var a = 0;
                    var b = 0;
                    a = point.points[0].xAxis - point.points[1].xAxis;
                    b = point.points[0].yAxis - point.points[1].yAxis;
                    var dist = Math.sqrt(a * a + b * b);
                    if (dist < 10) {
                        toRemove.push(index)
                    }

                });
                toRemove.forEach(function (i) {
                    this.pointers.splice(i, 1);
                }.bind(this))
            }
            this.setConnections(this);
        }.bind(this));
    }
    isPointerInPath() {
        var a = this.pointers.filter(function (point) {
            return (point.inPath === true) ? true : false;
        }) || [];

        if (a && a.length > 0) {
            return a;
        }
        return false;
    }
    inActivePoints() {
        this.pointers.filter(function (point) {
            point.active = false;
            point.clicked = false;
            point.points[0].lock = true;
            point.points[1].lock = true;
        });
    }
    inActiveObjects() {
        this.objects.filter(function (point) {
            point.active = false;
            return true;
        });
    }
    reDrawPoints() {
        this.pointers.forEach(function (point) {

            if (point !== null) {
                if (point.hasOwnProperty("active") && point.active) {
                    this.tools.delete = true;
                }
                point.render();
                point.isDblclick = false;
            }
        }.bind(this));
    }
    getPointer(_stg) {
        var self = this;
        class Pointer {
            constructor(stg) {
                this.points = [];
                this.active = false;
                this.lock = false;
                this.color = 'blue';
                this.inPath = false;
                this.stage = null;
                this.clicked = false;
                this.isMouseDown = false;
                this.stage = stg.stage;
                this.type = "normal";
                this.isMouseDown = stg.isMouseDown || true;
                this.bP = {
                    a: 0,
                    n: {
                        x: 0,
                        y: 0,
                        p: { x: 0, y: 0 },
                        n: { x: 0, y: 0 }
                    },
                    b: {
                        plx: 0,
                        ply: 0,
                        prx: 0,
                        pry: 0,
                        nlx: 0,
                        nly: 0,
                        nrx: 0,
                        nry: 0
                    },
                    p: {
                        x: 0,
                        y: 0,
                        p: { x: 0, y: 0 },
                        n: { x: 0, y: 0 }
                    },
                    sp: { x: 0, y: 0 },
                    ep: { x: 0, y: 0 },
                    ga: { x: 0, y: 0 },
                    ra: stage.utils.toRadian,
                    de: stage.utils.toDegree,
                    spc: 30,
                    aC: { f: "#ff3d3d", fh: "#ff0000", p: "#45ab26", ph: "#1b7500", n: "#999999", nh: "#393939", cc: null }
                };
                this.points.push(
                    {
                        lock: false,
                        xAxis: _stg.mouse.down.stageX,
                        yAxis: _stg.mouse.down.stageY,
                        lineCap: "circle",
                        object: {
                            id: null,
                            type: null,
                        }
                    }
                );
                this.points.push(
                    {
                        lock: false,
                        xAxis: _stg.mouse.move.stageX,
                        yAxis: _stg.mouse.move.stageY,
                        lineCap: "arrow",
                        object: {
                            id: null,
                            type: null,
                        }
                    }
                );
                this.stage.mouse.onClick(function () {
                    if (this.inPath) {
                        this.clicked = !this.clicked ? true : false;
                        this.active = !this.active ? true : false;
                    }
                    if (this.clicked) {
                        self.selected = { type: "pointer", id: this };
                    }
                    self.tools.save = true;
                }.bind(this));
                this.stage.mouse.onDown(function () {
                    this.isMouseDown = true;
                    self.tools.save = true;
                }.bind(this));
                this.stage.mouse.onUp(function () {
                    this.isMouseDown = false;
                    if (this.inPath) {
                        this.points[0].lock = true;
                        this.points[1].lock = true;
                    }
                    self.tools.save = true;

                }.bind(this));
                this.stage.mouse.onDblclick(function () {
                    this.isDblclick = true;
                }.bind(this));

            }

            setType() {
                // re-program
                var p1 = this.points[0];
                var p2 = this.points[1];
                var _self = this;
                if (p1.object.type === null || p2.object.type === null) {
                    return;
                }
                // Normal Line
                if ((p1.object.type === "start-state" && p1.lineCap === "circle") && ((p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment") && p2.lineCap === "arrow")) {
                   
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    if (selfObj.length >= 2) {
                        self.pointers = self.pointers.filter(function (p) {
                            if (self.stage.utils.compareObjects(p, selfObj[1])) {
                                return false;
                            }
                            return true;
                        });
                    }
                   
                    selfObj.slice(1, selfObj.length);
                    return true;
                }
                var fromState = ((p1.object.type === "start-state" || p1.object.type === "api" || p1.object.type === "automation" || p1.object.type === "email" || p1.object.type === "assign" || p1.object.type === "caseassignment") && p1.lineCap === "arrow" && p2.lineCap === "circle" && (p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment"));
                var toState = ((p2.object.type === "end-state" || p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment") && p2.lineCap === "arrow" && p1.lineCap === "circle" && (p1.object.type === "api" || p1.object.type === "automation" || p1.object.type === "email" || p1.object.type === "assign" || p1.object.type === "caseassignment"));
                if (fromState || toState) {
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    if (selfObj.length >= 2) {
                        if (selfObj[0].type === "pass") {

                            selfObj[1].type = "fail";

                        } else if (selfObj[0].type === "fail") {

                            selfObj[1].type = "pass";

                        } else {
                            selfObj[0].type = "pass";
                            selfObj[1].type = "fail";
                        }
                    } else {
                        _self.type = "pass";
                    }
                    if (selfObj.length >= 3) {
                        self.pointers = self.pointers.filter(function (p) {
                            if (self.stage.utils.compareObjects(p, selfObj[2])) {
                                return false;
                            }
                            return true;
                        });
                    }
                    selfObj.slice(2, selfObj.length);
                }
                else {
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    if (selfObj.length >= 0) {
                        self.pointers = self.pointers.filter(function (p) {
                            if (self.stage.utils.compareObjects(p, selfObj[0])) {
                                return false;
                            }
                            return true;
                        });
                    }
                    selfObj.slice(0, selfObj.length);
                    return false;
                }
            }
            render() {
                var fullSpace = 14;
                var halfSize = fullSpace / 2.5;
                var halfOfHalfSize = fullSpace / 14;
                var ctx = stage.ctx2d;

                this.bP.ga = Math.atan2(this.points[0].yAxis - this.points[1].yAxis, this.points[0].xAxis - this.points[1].xAxis);
                /* Start: Pass / Failed / Block */
                this.setType();
                /* End: Pass / Failed / Block */
                if (this.isMouseDown && this.points[0].lock === false) {
                    this.points[0].xAxis = stage.mouse.move.stageX;
                    this.points[0].yAxis = stage.mouse.move.stageY;
                    self.tools.save = true;
                }
                if (this.isMouseDown && this.points[1].lock === false) {
                    this.points[1].xAxis = stage.mouse.move.stageX;
                    this.points[1].yAxis = stage.mouse.move.stageY;
                    self.tools.save = true;
                }

                this.bP.p.x = this.points[0].xAxis;
                this.bP.p.y = this.points[0].yAxis;
                this.bP.n.x = this.points[1].xAxis;
                this.bP.n.y = this.points[1].yAxis;

                if (self.getDistance(this.points[0]) < 30) {
                    var dis = self.getDistance(this.points[0]);
                    if (this.isMouseDown && !this.globalUnlockPoints) dis = 0;
                    this.bP.p.x = this.bP.p.x - Math.cos(this.bP.ga) * (this.bP.spc - dis);
                    this.bP.p.y = this.bP.p.y - Math.sin(this.bP.ga) * (this.bP.spc - dis);
                }
                if (self.getDistance(this.points[1]) < 30) {
                    var dis = self.getDistance(this.points[1]);
                    if (this.isMouseDown && !this.globalUnlockPoints) dis = 0;
                    this.bP.n.x = this.bP.n.x + Math.cos(this.bP.ga) * (this.bP.spc - dis);
                    this.bP.n.y = this.bP.n.y + Math.sin(this.bP.ga) * (this.bP.spc - dis);
                }
                this.bP.ga = Math.atan2(this.bP.n.y - this.bP.p.y, this.bP.n.x - this.bP.p.x);
                var x = this.bP.p.x + Math.cos(this.bP.ga) * halfOfHalfSize;
                var y = this.bP.p.y + Math.sin(this.bP.ga) * halfOfHalfSize;
                ctx.lineCap = "square";
                ctx.strokeStyle = "red";
                this.bP.a = this.bP.ra(90) + this.bP.ga;
                this.bP.p.n.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p.n.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.plx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.ply = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(-90) + this.bP.ga;
                this.bP.p.p.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p.p.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.prx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.pry = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(180) + this.bP.ga;
                this.bP.p["x1"] = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p["y1"] = y + Math.sin(this.bP.a) * halfSize;
                this.bP.p.x = this.bP.p.x1;
                this.bP.p.y = this.bP.p.y1;
                /*
                //posetive arrow
                ctx.beginPath();
                ctx.moveTo(this.bP.p.x,this.bP.p.y);
                ctx.lineTo(this.bP.p.n.x,this.bP.p.n.y);
                ctx.lineTo(this.bP.p.p.x,this.bP.p.p.y);
                ctx.lineTo(this.bP.p.x1,this.bP.p.y1);
                ctx.lineTo(this.bP.p.n.x,this.bP.p.n.y);
                ctx.strokeStyle="#ff0fff";
                ctx.stroke();
                ctx.closePath();
                
                //box
                ctx.beginPath();
                ctx.moveTo(this.bP.b.prx,this.bP.b.pry);
                ctx.lineTo(this.bP.b.plx,this.bP.b.ply);
                ctx.lineTo(this.bP.b.nlx,this.bP.b.nly);
                ctx.lineTo(this.bP.b.nrx,this.bP.b.nry);
                ctx.closePath();
                ctx.strokeStyle="#000";
                ctx.stroke();
                
                //negative
                ctx.moveTo(this.bP.n.x,this.bP.n.y);
                ctx.beginPath();
                ctx.lineTo(this.bP.n.x,this.bP.n.y);
                ctx.lineTo(this.bP.n.n.x,this.bP.n.n.y);
                ctx.lineTo(this.bP.n.p.x,this.bP.n.p.y);
                ctx.lineTo(this.bP.n.x1,this.bP.n.y1);
                ctx.lineTo(this.bP.n.n.x,this.bP.n.n.y);
                ctx.strokeStyle="#ff0fff";
                ctx.closePath();
                ctx.stroke();
                */

                x = this.bP.n.x - Math.cos(this.bP.ga) * halfOfHalfSize;
                y = this.bP.n.y - Math.sin(this.bP.ga) * halfOfHalfSize;

                this.bP.a = this.bP.ra(90) + this.bP.ga;
                this.bP.n.n.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n.n.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.nlx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.nly = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(-90) + this.bP.ga;
                this.bP.n.p.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n.p.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.nrx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.nry = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(0) + this.bP.ga;
                this.bP.n["x1"] = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n["y1"] = y + Math.sin(this.bP.a) * halfSize;
                this.bP.n.x = this.bP.n.x1;
                this.bP.n.y = this.bP.n.y1;
                ctx.beginPath();
                if (this.points[0].lineCap === "arrow") {
                    ctx.moveTo(this.bP.p.x1, this.bP.p.y1);
                    ctx.lineTo(this.bP.p.n.x, this.bP.p.n.y);
                }
                if (this.points[0].lineCap === "circle") {
                    var radius = halfSize / 1.4;
                    var x = this.bP.p.x + Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.p.y + Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(x, y);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(x, y);
                }
                if (this.points[0].lineCap === "round") {
                    var radius = halfSize / 5;
                    var x = this.bP.p.x + Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.p.y + Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(x, y);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.plx, this.bP.b.ply);

                }
                ctx.lineTo(this.bP.b.plx, this.bP.b.ply);
                ctx.lineTo(this.bP.b.nlx, this.bP.b.nly);
                if (this.points[1].lineCap === "arrow") {
                    ctx.lineTo(this.bP.n.n.x, this.bP.n.n.y);
                    ctx.lineTo(this.bP.n.x1, this.bP.n.y1);
                    ctx.lineTo(this.bP.n.p.x, this.bP.n.p.y);
                }
                if (this.points[1].lineCap === "circle") {
                    var radius = halfSize / 1.4;
                    var x = this.bP.n.x - Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.n.y - Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                }
                if (this.points[1].lineCap === "round") {
                    var radius = halfSize / 5;
                    var x = this.bP.n.x - Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.n.y - Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);

                }

                ctx.lineTo(this.bP.b.nrx, this.bP.b.nry);
                ctx.lineTo(this.bP.b.prx, this.bP.b.pry);
                if (this.points[0].lineCap === "arrow") {
                    ctx.lineTo(this.bP.p.p.x, this.bP.p.p.y);
                    ctx.lineTo(this.bP.p.x1, this.bP.p.y1);
                }
                if (this.points[0].lineCap === "round") {
                    // do nothing in right side
                }


                if (this.type === "normal") {
                    this.bP.aC.cc = this.bP.aC.n;
                } else if (this.type === "pass") {
                    this.bP.aC.cc = this.bP.aC.p;
                } else if (this.type === "fail") {
                    this.bP.aC.cc = this.bP.aC.f;
                }
                //ctx.fillStyle = this.bP.aC.cc;
                this.inPath = ctx.isPointInPath(stage.mouse.move.stageX * stage.utils.getDpi(), stage.mouse.move.stageY * stage.utils.getDpi());
                if (this.active || this.inPath) {
                    if (this.type === "normal") {
                        this.bP.aC.cc = this.bP.aC.nh;
                    } else if (this.type === "pass") {
                        this.bP.aC.cc = this.bP.aC.ph;
                    } else if (this.type === "fail") {
                        this.bP.aC.cc = this.bP.aC.fh;
                    }

                }
                ctx.fillStyle = this.bP.aC.cc;
                if (this.inPath) {
                    var a = this.bP.p.x - stage.mouse.move.stageX;
                    var b = this.bP.p.y - stage.mouse.move.stageY;
                    var posetive = Math.sqrt(a * a + b * b);
                    if (posetive < 12) {
                        this.points[0].lock = false;
                        this.points[1].lock = true;
                        if (this.isDblclick && this.points[0].object.type === "state" && this.points[1].object.type === "state") {
                            if (this.points[0].lineCap === "circle") {
                                this.points[0].lineCap = "arrow";
                            } else {
                                this.points[0].lineCap = "circle";
                            }
                        }
                    }
                    a = this.bP.n.x - stage.mouse.move.stageX;
                    b = this.bP.n.y - stage.mouse.move.stageY;
                    var negative = Math.sqrt(a * a + b * b);
                    if (negative < 12) {
                        this.points[1].lock = false;
                        this.points[0].lock = true;
                        if (this.isDblclick && this.points[1].object.type === "state" && this.points[0].object.type === "state") {
                            if (this.points[1].lineCap === "circle") {
                                this.points[1].lineCap = "arrow";
                            } else {
                                this.points[1].lineCap = "circle";
                            }
                        }
                    }
                    if (stage.cursors.active === null && stage.cursors.old === null) {
                        stage.cursors.old = stage.canvas.style.cursor === "" ? "pointer" : stage.canvas.style.cursor;
                        stage.cursors.active = stage.cursors.default();
                    }
                    if (stage.mouse.isDown) {
                        stage.cursors.active = stage.cursors.default();
                    }
                } else {
                    if (stage.cursors.active !== null && stage.cursors.old != null) {
                        stage.canvas.style.cursor = stage.cursors.old;
                        stage.cursors.old = null;
                        stage.cursors.active = null;
                    }
                }

                ctx.closePath();
                ctx.fill();
                ctx.beginPath();
                ctx.arc(this.bP.p.x, this.bP.p.y, 2, 0, Math.PI * 2);
                ctx.closePath();
                ctx.strokeStyle = "#ff0fff";
                //ctx.stroke();
                ctx.beginPath();
                ctx.arc(this.bP.n.x, this.bP.n.y, 2, 0, Math.PI * 2);
                ctx.closePath();
                ctx.strokeStyle = "#ff0fff";
                //ctx.stroke();
            }
        };
        var p = new Pointer({ stage: _stg, isMouseDown: true });
        p.render();
        return p;
    }
    isPointerInObjectPath() {
        var ToReturn = false;
        this.objects.forEach(function (obj) {

            if (obj.inPath && !ToReturn) {
                ToReturn = true;
            }
        });
        return ToReturn;
    }
    inActiveObject() {
        this.objects.forEach(function (obj) {
            obj.active = false;
        });
    }
    getDistance(p) {
        var obj = this.findObject(p.object.id);
        if (obj !== null) {
            var a = obj.x - p.xAxis;
            var b = obj.y - p.yAxis;
            return Math.sqrt(a * a + b * b);
        }
        return 100;
    }
    findObject(id) {
        if (parseInt(id) === 0) return null;
        var ob = this.objects.filter(function (o) {
            if (o.id === id) {
                return true;
            }
        })
        if (ob.length > 0) return ob[0];
        return null;

    }
    getObject(_stg) {
        var self = this;
        class Object {

            constructor(stg) {
                this.active = false;
                this.lock = true;
                this.stroke = null;
                this.fill = null;
                this.stage = null;
                this.type = null;
                this.inPath = false;
                this.x = 0;
                this.y = 0;
                this.size = 20;
                this.color = null;
                this.title = "";
                this.countdown = {};
                //
                this.stage = stg.stage;
                this.x = stg.x || 0;
                this.y = stg.y || 0;
                this.size = stg.size || 0;
                this.type = stg.type || "state";
                this.shape = stg.shape || "circle";
                this.icon = stg.icon || null;
                this.stroke = stg.stroke || null;
                this.fill = stg.fill || null;
                this.color = stg.color || "#ebebeb"
                this.title = stg.title || "New State";
                this.id = stg.id;
                this.isMouseDown = false;
                this.stage.mouse.onClick(function () {
                    if (this.inPath) {
                        this.clicked = !this.clicked ? true : false;
                        this.active = !this.active ? true : false;
                        self.tools.save = true;

                    }
                }.bind(this));
                this.stage.mouse.onDown(function () {
                    this.isMouseDown = true;
                    if (this.isMouseDown && this.inPath) {
                        this.lock = false;
                        self.tools.save = true;
                        self.unLockPoints({ "type": this.type, "id": this.id })
                    }
                }.bind(this));
                this.stage.mouse.onUp(function () {
                    this.isMouseDown = false;
                    this.lock = true;
                    self.tools.save = true;
                    self.setConnections(this);
                    self.lockPoints();
                }.bind(this));

            }

            render() {
                this.countdown["oH"] = typeof (this.countdown["oH"]) === 'undefined' ? (this.countdown["oH"] = 0) : this.countdown.oH;
                if (this.countdown.oH < 24) {
                    //this.countdown.oH++;
                }
                var ctx = stage.ctx2d;
                if (!this.lock) {
                    this.x = stage.mouse.move.stageX || 0;
                    this.y = stage.mouse.move.stageY || 0;
                }

                var XN = 0;
                if (this.countdown.oH >= 0) {
                    XN = stage.utils.map(this.countdown.oH, 0, 24, 0, 12);
                } else {
                    XN = stage.utils.map(this.countdown.oH, 0, 24, -12, 0);
                }

                ctx.lineWidth = 1;

                if (this.shape === 'circle') {
                    ctx.beginPath();
                    ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
                    ctx.closePath();
                    ctx.shadowColor = "rgba(0,0,0,.4)";
                    ctx.shadowBlur = 10
                    ctx.shadowOffsetX = 0;
                    ctx.shadowOffsetY = 0;
                    ctx.beginPath();
                    ctx.arc(this.x, this.y, this.size + XN, 0, Math.PI * 2);
                    ctx.closePath();
                    ctx.shadowColor = "rgba(0,0,0,.4)";
                    ctx.shadowBlur = 10
                    ctx.shadowOffsetX = 0;
                    ctx.shadowOffsetY = 0;
                    this.inPath = ctx.isPointInPath(stage.mouse.move.stageX * stage.utils.getDpi(), stage.mouse.move.stageY * stage.utils.getDpi());
                    if (this.stroke) {
                        ctx.strokeStyle = this.stroke;
                        ctx.stroke();
                    }
                    if (this.active) {
                        ctx.strokeStyle = "#ff0000";
                        ctx.stroke();
                    }
                    if (this.fill) {
                        ctx.fillStyle = this.fill;
                        ctx.fill();
                    }
                    ctx.shadowBlur = 0
                    ctx.lineWidth = 0;
                }
                if (this.shape === 'rect') {
                    ctx.beginPath();
                    ctx.rect(this.x - (this.size / 2), this.y - (this.size / 2), this.size, this.size);
                    ctx.closePath();
                    ctx.shadowColor = "rgba(0,0,0,.4)";
                    ctx.shadowBlur = 10
                    ctx.shadowOffsetX = 0;
                    ctx.shadowOffsetY = 0;
                    ctx.lineWidth = 1;
                    this.inPath = ctx.isPointInPath(stage.mouse.move.stageX * stage.utils.getDpi(), stage.mouse.move.stageY * stage.utils.getDpi());
                    if (this.stroke) {
                        ctx.strokeStyle = this.stroke;
                        ctx.stroke();
                    }
                    if (this.active) {
                        ctx.strokeStyle = "#ff0000";
                        ctx.stroke();
                    }
                    if (this.fill) {
                        ctx.fillStyle = this.fill;
                        ctx.fill();
                    }
                    ctx.shadowBlur = 0;
                    ctx.lineWidth = 0;
                }
                var icon = {
                    x: this.x,
                    y: this.y,
                    title: this.title,
                    size: ctx.measureText(this.icon),
                    titleX: function () {
                        return icon.x + icon.size.width + 20;
                    },
                    titleY: function () {
                        return icon.y;
                    }
                };

                ctx.fillStyle = this.color;
                ctx.font = "20px remixicon";
                ctx.fillText(this.icon, icon.x - (icon.size.width), icon.y + (icon.size.width));
                ctx.fillStyle = this.color;
                ctx.font = "14px Lato, Arial, sans-serif";
                ctx.fillText(icon.title, icon.titleX(), icon.titleY() - 5);
                ctx.font = "13px 600 Lato, Arial, sans-serif";
                ctx.fillText("Type: " + this.type, icon.titleX(), icon.titleY() + 15);
                if (this.isMouseDown && this.inPath) {
                    this.lock = false;
                    self.selected = { type: this.type, id: this.id };
                }

                if (this.inPath) {
                    if (stage.cursors.active === null && stage.cursors.old === null) {
                        stage.cursors.old = stage.canvas.style.cursor === "" ? "pointer" : stage.canvas.style.cursor;
                        stage.cursors.active = stage.cursors.grab();
                    }
                    if (stage.mouse.isDown) {
                        stage.cursors.active = stage.cursors.grabbing();
                    }
                }

            };
        }
        var p = new Object(_stg);
        p.render();
        return p;
    }
    reDrawObjects() {
        this.objects.forEach(function (obj) {

            if (obj.active) {
                this.tools.delete = true;
                this.selected = { type: obj.type, id: obj.id };
            }
            obj.render();
        }.bind(this));
    }
    insertObject(e) {

        var objs = this.getObject({ stage: stage, x: e.x, y: e.y, shape: e.shape, fill: "#f4f5f6", icon: e.icon, active: false, color: "#585a5c", id: e.id, type: e.type, title: e.title /*+ " - " + e.id*/, size: e.size });
        this.objects.push(objs);
        this.reDrawObjects();
        $('#wf-' + objs.type).modal('hide');
    }
    pushObject(e) {

        var objs = this.getObject({ stage: stage, x: e.x, y: e.y, shape: e.shape, fill: "#f4f5f6", icon: e.icon, active: false, color: "#585a5c", id: e.id, type: e.type, title: e.title.split("-")[0], size: e.size });
        wf.objects.push(objs);

    }
    setPointer(_args, _stage) {
        var self = this;
        class Pointer {
            constructor(stg, _stage) {
                this.points = [];
                this.active = false;
                this.lock = false;
                this.color = 'blue';
                this.inPath = false;
                this.stage = null;
                this.clicked = false;
                this.isMouseDown = false;
                this.isDblclick = false;
                this.type = stg.type;
                this.stage = _stage;
                this.isMouseDown = stg.isMouseDown;
                this.bP = {
                    a: 0,
                    n: {
                        x: 0,
                        y: 0,
                        p: { x: 0, y: 0 },
                        n: { x: 0, y: 0 }
                    },
                    b: {
                        plx: 0,
                        ply: 0,
                        prx: 0,
                        pry: 0,
                        nlx: 0,
                        nly: 0,
                        nrx: 0,
                        nry: 0
                    },
                    p: {
                        x: 0,
                        y: 0,
                        p: { x: 0, y: 0 },
                        n: { x: 0, y: 0 }
                    },
                    sp: { x: 0, y: 0 },
                    ep: { x: 0, y: 0 },
                    ga: { x: 0, y: 0 },
                    ra: stage.utils.toRadian,
                    de: stage.utils.toDegree,
                    spc: 30,
                    aC: { f: "#ff3d3d", fh: "#ff0000", p: "#45ab26", ph: "#1b7500", n: "#999999", nh: "#393939", cc: null }
                };
                this.points.push(
                    {
                        lock: stg.points[0].lock,
                        xAxis: stg.points[0].xAxis,
                        yAxis: stg.points[0].yAxis,
                        lineCap: stg.points[0].lineCap,
                        object: {
                            id: stg.points[0].object.id,
                            type: stg.points[0].object.type,
                        }
                    }
                );
                this.points.push(
                    {
                        lock: stg.points[1].lock,
                        xAxis: stg.points[1].xAxis,
                        yAxis: stg.points[1].yAxis,
                        lineCap: stg.points[1].lineCap,
                        object: {
                            id: stg.points[1].object.id,
                            type: stg.points[1].object.type,
                        }
                    }
                );
                this.stage.mouse.onClick(function () {
                    if (this.inPath) {
                        this.clicked = !this.clicked ? true : false;
                        this.active = !this.active ? true : false;
                    }
                    self.tools.save = true;
                }.bind(this));
                this.stage.mouse.onDown(function () {
                    this.isMouseDown = true;
                    self.tools.save = true;
                }.bind(this));
                this.stage.mouse.onUp(function () {
                    this.isMouseDown = false;

                    if (this.inPath) {
                        this.points[0].lock = true;
                        this.points[1].lock = true;
                    }
                    self.tools.save = true;
                }.bind(this));
                this.stage.mouse.onDblclick(function () {
                    this.isDblclick = true;
                }.bind(this));
            }
            setType() {
                // re-program
                var p1 = this.points[0];
                var p2 = this.points[1];
                var _self = this;
                if (p1.object.type === null || p2.object.type === null) {
                    return;
                }
                // Normal Line

                if ((p1.object.type === "start-state" && p1.lineCap === "circle") && ((p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment") && p2.lineCap === "arrow")) {
                    this.type = "normal";
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    if (selfObj.length >= 2) {
                        self.pointers = self.pointers.filter(function (p) {
                            if (self.stage.utils.compareObjects(p, selfObj[1])) {
                                return false;
                            }
                            return true;
                        });
                    }

                    selfObj.slice(1, selfObj.length);
                    return true;
                }
                var fromState = ((p1.object.type === "start-state" || p1.object.type === "api" || p1.object.type === "automation" || p1.object.type === "email" || p1.object.type === "assign" || p1.object.type === "caseassignment") && p1.lineCap === "arrow" && p2.lineCap === "circle" && (p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment"));
                var toState = ((p2.object.type === "end-state" || p2.object.type === "api" || p2.object.type === "automation" || p2.object.type === "email" || p2.object.type === "assign" || p2.object.type === "caseassignment") && p2.lineCap === "arrow" && p1.lineCap === "circle" && (p1.object.type === "api" || p1.object.type === "automation" || p1.object.type === "email" || p1.object.type === "assign" || p1.object.type === "caseassignment"));


                if (fromState || toState) {
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    if (selfObj.length >= 2) {
                        if (selfObj[0].type === "pass") {

                            selfObj[1].type = "fail";

                        } else if (selfObj[0].type === "fail") {

                            selfObj[1].type = "pass";

                        } else {
                            selfObj[0].type = "pass";
                            selfObj[1].type = "fail";
                        }
                    } else {
                        _self.type = "pass";
                    }
                    if (selfObj.length >= 3) {
                        self.pointers = self.pointers.filter(function (p) {
                            if (self.stage.utils.compareObjects(p, selfObj[2])) {
                                return false;
                            }
                            return true;
                        });
                    }
                    selfObj.slice(2, selfObj.length);
                }
                else {
                    this.type = "";
                    var selfObj = new Array();
                    self.pointers.forEach(function (p) {
                        if (self.stage.utils.compareObjects(p, _self)) {
                            selfObj.push(p);
                        } else {
                            if ((p.points[0].object.type !== "start-state" && p.points[0].lineCap === "circle" && p.points[0].object.id === p1.object.id) || (p.points[1].object.type !== "start-state" && p.points[1].lineCap === "circle" && p.points[1].object.id === p2.object.id)) {
                                selfObj.push(p);
                            }
                        }
                    });
                    selfObj.slice(0, selfObj.length);
                    return false;
                }
            }

            render() {
                var fullSpace = 14;
                var halfSize = fullSpace / 2.5;
                var halfOfHalfSize = fullSpace / 14;
                var ctx = stage.ctx2d;

                this.bP.ga = Math.atan2(this.points[0].yAxis - this.points[1].yAxis, this.points[0].xAxis - this.points[1].xAxis);
                /* Start: Pass / Failed / Block */
                this.setType();
                /* End: Pass / Failed / Block */
                if (this.isMouseDown && this.points[0].lock === false) {
                    this.points[0].xAxis = stage.mouse.move.stageX;
                    this.points[0].yAxis = stage.mouse.move.stageY;
                    self.tools.save = true;
                }
                if (this.isMouseDown && this.points[1].lock === false) {
                    this.points[1].xAxis = stage.mouse.move.stageX;
                    this.points[1].yAxis = stage.mouse.move.stageY;
                    self.tools.save = true;
                }

                this.bP.p.x = this.points[0].xAxis;
                this.bP.p.y = this.points[0].yAxis;
                this.bP.n.x = this.points[1].xAxis;
                this.bP.n.y = this.points[1].yAxis;

                if (self.getDistance(this.points[0]) < 30) {
                    var dis = self.getDistance(this.points[0]);
                    if (this.isMouseDown && !this.globalUnlockPoints) dis = 0;
                    this.bP.p.x = this.bP.p.x - Math.cos(this.bP.ga) * (this.bP.spc - dis);
                    this.bP.p.y = this.bP.p.y - Math.sin(this.bP.ga) * (this.bP.spc - dis);
                }
                if (self.getDistance(this.points[1]) < 30) {
                    var dis = self.getDistance(this.points[1])
                    if (this.isMouseDown && !this.globalUnlockPoints) dis = 0;
                    this.bP.n.x = this.bP.n.x + Math.cos(this.bP.ga) * (this.bP.spc - dis);
                    this.bP.n.y = this.bP.n.y + Math.sin(this.bP.ga) * (this.bP.spc - dis);
                }
                this.bP.ga = Math.atan2(this.bP.n.y - this.bP.p.y, this.bP.n.x - this.bP.p.x);
                var x = this.bP.p.x + Math.cos(this.bP.ga) * halfOfHalfSize;
                var y = this.bP.p.y + Math.sin(this.bP.ga) * halfOfHalfSize;
                ctx.lineCap = "square";
                ctx.strokeStyle = "red";
                this.bP.a = this.bP.ra(90) + this.bP.ga;
                this.bP.p.n.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p.n.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.plx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.ply = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(-90) + this.bP.ga;
                this.bP.p.p.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p.p.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.prx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.pry = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(180) + this.bP.ga;
                this.bP.p["x1"] = x + Math.cos(this.bP.a) * halfSize;
                this.bP.p["y1"] = y + Math.sin(this.bP.a) * halfSize;
                this.bP.p.x = this.bP.p.x1;
                this.bP.p.y = this.bP.p.y1;
                /*
                //posetive arrow
                ctx.beginPath();
                ctx.moveTo(this.bP.p.x,this.bP.p.y);
                ctx.lineTo(this.bP.p.n.x,this.bP.p.n.y);
                ctx.lineTo(this.bP.p.p.x,this.bP.p.p.y);
                ctx.lineTo(this.bP.p.x1,this.bP.p.y1);
                ctx.lineTo(this.bP.p.n.x,this.bP.p.n.y);
                ctx.strokeStyle="#ff0fff";
                ctx.stroke();
                ctx.closePath();
                
                //box
                ctx.beginPath();
                ctx.moveTo(this.bP.b.prx,this.bP.b.pry);
                ctx.lineTo(this.bP.b.plx,this.bP.b.ply);
                ctx.lineTo(this.bP.b.nlx,this.bP.b.nly);
                ctx.lineTo(this.bP.b.nrx,this.bP.b.nry);
                ctx.closePath();
                ctx.strokeStyle="#000";
                ctx.stroke();
                
                //negative
                ctx.moveTo(this.bP.n.x,this.bP.n.y);
                ctx.beginPath();
                ctx.lineTo(this.bP.n.x,this.bP.n.y);
                ctx.lineTo(this.bP.n.n.x,this.bP.n.n.y);
                ctx.lineTo(this.bP.n.p.x,this.bP.n.p.y);
                ctx.lineTo(this.bP.n.x1,this.bP.n.y1);
                ctx.lineTo(this.bP.n.n.x,this.bP.n.n.y);
                ctx.strokeStyle="#ff0fff";
                ctx.closePath();
                ctx.stroke();
                */

                x = this.bP.n.x - Math.cos(this.bP.ga) * halfOfHalfSize;
                y = this.bP.n.y - Math.sin(this.bP.ga) * halfOfHalfSize;

                this.bP.a = this.bP.ra(90) + this.bP.ga;
                this.bP.n.n.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n.n.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.nlx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.nly = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(-90) + this.bP.ga;
                this.bP.n.p.x = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n.p.y = y + Math.sin(this.bP.a) * halfSize;
                this.bP.b.nrx = x + Math.cos(this.bP.a) * halfOfHalfSize;
                this.bP.b.nry = y + Math.sin(this.bP.a) * halfOfHalfSize;
                this.bP.a = this.bP.ra(0) + this.bP.ga;
                this.bP.n["x1"] = x + Math.cos(this.bP.a) * halfSize;
                this.bP.n["y1"] = y + Math.sin(this.bP.a) * halfSize;
                this.bP.n.x = this.bP.n.x1;
                this.bP.n.y = this.bP.n.y1;
                ctx.beginPath();
                if (this.points[0].lineCap === "arrow") {
                    ctx.moveTo(this.bP.p.x1, this.bP.p.y1);
                    ctx.lineTo(this.bP.p.n.x, this.bP.p.n.y);
                }
                if (this.points[0].lineCap === "circle") {
                    var radius = halfSize / 1.4;
                    var x = this.bP.p.x + Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.p.y + Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(x, y);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(x, y);
                }
                if (this.points[0].lineCap === "round") {
                    var radius = halfSize / 5;
                    var x = this.bP.p.x + Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.p.y + Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(x, y);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.plx, this.bP.b.ply);

                }
                ctx.lineTo(this.bP.b.plx, this.bP.b.ply);
                ctx.lineTo(this.bP.b.nlx, this.bP.b.nly);
                if (this.points[1].lineCap === "arrow") {
                    ctx.lineTo(this.bP.n.n.x, this.bP.n.n.y);
                    ctx.lineTo(this.bP.n.x1, this.bP.n.y1);
                    ctx.lineTo(this.bP.n.p.x, this.bP.n.p.y);
                }
                if (this.points[1].lineCap === "circle") {
                    var radius = halfSize / 1.4;
                    var x = this.bP.n.x - Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.n.y - Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                }
                if (this.points[1].lineCap === "round") {
                    var radius = halfSize / 5;
                    var x = this.bP.n.x - Math.cos(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    var y = this.bP.n.y - Math.sin(this.bP.ra(0) + this.bP.ga) * (halfSize / 1.2);
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);
                    for (var angle = 0; angle <= 360; angle++) {
                        var newX = radius * Math.cos(this.bP.de(angle) + this.bP.ga);
                        var newY = radius * Math.sin(this.bP.de(angle) + this.bP.ga);
                        ctx.lineTo(newX + x, newY + y);
                    }
                    ctx.moveTo(this.bP.b.nlx, this.bP.b.nly);

                }

                ctx.lineTo(this.bP.b.nrx, this.bP.b.nry);
                ctx.lineTo(this.bP.b.prx, this.bP.b.pry);
                if (this.points[0].lineCap === "arrow") {
                    ctx.lineTo(this.bP.p.p.x, this.bP.p.p.y);
                    ctx.lineTo(this.bP.p.x1, this.bP.p.y1);
                }
                if (this.points[0].lineCap === "round") {
                    // do nothing in right side
                }


                if (this.type === "normal") {
                    this.bP.aC.cc = this.bP.aC.n;
                } else if (this.type === "pass") {
                    this.bP.aC.cc = this.bP.aC.p;
                } else if (this.type === "fail") {
                    this.bP.aC.cc = this.bP.aC.f;
                }
                //ctx.fillStyle = this.bP.aC.cc;
                this.inPath = ctx.isPointInPath(stage.mouse.move.stageX * stage.utils.getDpi(), stage.mouse.move.stageY * stage.utils.getDpi());
                if (this.active || this.inPath) {
                    if (this.type === "normal") {
                        this.bP.aC.cc = this.bP.aC.nh;
                    } else if (this.type === "pass") {
                        this.bP.aC.cc = this.bP.aC.ph;
                    } else if (this.type === "fail") {
                        this.bP.aC.cc = this.bP.aC.fh;
                    }

                }
                ctx.fillStyle = this.bP.aC.cc;
                if (this.inPath) {
                    var a = this.bP.p.x - stage.mouse.move.stageX;
                    var b = this.bP.p.y - stage.mouse.move.stageY;
                    var posetive = Math.sqrt(a * a + b * b);
                    if (posetive < 12) {
                        this.points[0].lock = false;
                        this.points[1].lock = true;
                    }
                    a = this.bP.n.x - stage.mouse.move.stageX;
                    b = this.bP.n.y - stage.mouse.move.stageY;
                    var negative = Math.sqrt(a * a + b * b);
                    if (negative < 12) {
                        this.points[1].lock = false;
                        this.points[0].lock = true;
                    }
                    if (stage.cursors.active === null && stage.cursors.old === null) {
                        stage.cursors.old = stage.canvas.style.cursor === "" ? "pointer" : stage.canvas.style.cursor;
                        stage.cursors.active = stage.cursors.default();
                    }
                    if (stage.mouse.isDown) {
                        stage.cursors.active = stage.cursors.default();
                    }
                } else {
                    if (stage.cursors.active !== null && stage.cursors.old !== null) {
                        stage.canvas.style.cursor = stage.cursors.old;
                        stage.cursors.old = null;
                        stage.cursors.active = null;
                    }
                }

                ctx.closePath();
                ctx.fill();
                ctx.beginPath();
                ctx.arc(this.bP.p.x, this.bP.p.y, 2, 0, Math.PI * 2);
                ctx.closePath();
                ctx.strokeStyle = "#ff0fff";
                //ctx.stroke();
                ctx.beginPath();
                ctx.arc(this.bP.n.x, this.bP.n.y, 2, 0, Math.PI * 2);
                ctx.closePath();
                ctx.strokeStyle = "#ff0fff";
                //ctx.stroke();
            }
        };
        return new Pointer(_args, _stage);
    }
    pushPoint(e) {
        var pts = this.setPointer(e, stage);

        wf.pointers.push(pts);
    }
    deleteAll() {
        this.objects = [];
        this.pointers = [];

    }
    deleteActive() {
        var oj = { type: null, id: null };
        var objs = this.objects.filter(function (obj) {
            if (obj.active !== true) {
                oj.id = obj.id;
                oj.type = obj.type;
            }
            else {
                if (obj.type === "state") {
                    wf.vars.deleteStateForForm = wf.vars.deleteStateForForm + "," + obj.id;
                }
            }
            return (obj.active === false) ? true : false;
        });
        this.objects = objs;
        var points = this.pointers.filter(function (point) {
            if (point.active === false || (point.points[0].id !== null && point.points[1].id !== null && ((point.points[0].id === oj.id && point.points[0].type === oj.type) || (point.points[1].id === oj.id && point.points[1].type === oj.type)))) {
                return true;
            } else {
                return false;
            }

        });
        this.pointers = points;

    }
    render(stage, canvas, ctx) {
        this.reDrawPoints();
        this.reDrawObjects();
        if (stage.cursors.active !== null && stage.cursors.old !== null) {
            stage.canvas.style.cursor = stage.cursors.active;
        }
        if (this.tools.delete) {
            $('[data-tool="delete"]').attr("disabled", false);
        }
        if (this.tools.save) {
            $('[data-tool="save"]').attr("disabled", false);
        }
        this.isDblclick = false;
        // console.log(this.selected)
    };
    export() {
        var ob = { "point": [], "object": [] };
        this.pointers.forEach(function (point) {
            var p = Object.assign({}, point);
            p.stage = null
            ob.point.push(p);
        })
        this.objects.forEach(function (objs) {
            var p = Object.assign({}, objs);
            p.stage = null
            ob.object.push(p);
        })
        return ob;
    }
    import(e) {
        this.pointers = e.point;
        this.objects = e.object;
    }
    unLockPoints(e) {
        this.globalUnlockPoints = true;
        this.pointers.forEach(function (point) {
            point.points[0].lock = false;
            point.points[1].lock = false;
            //  console.log(point);
            if (e.id === point.points[0].object.id && e.type === point.points[0].object.type) {
                point.points[0].lock = false;
            } else {
                point.points[0].lock = true;
            }
            if (e.id === point.points[1].object.id && e.type === point.points[1].object.type) {
                point.points[1].lock = false;
            } else {
                point.points[1].lock = true;
            }
        })

    }
    lockPoints() {
        this.globalUnlockPoints = false;
        this.pointers.forEach(function (point) {
            point.points[0].lock = true;
            point.points[1].lock = true;
        })
    }
    setConnections() {
        var self = this;
        var newPoints = self.pointers.filter(function (point) {
            point.points.forEach(function (p) {
                //if (p.object.id === "" || p.object.id ===null) {
                var closest = { id: null, distance: 1200, type: null, cx: null, cy: null };
                self.objects.forEach(function (v, index) {
                    closest.cx = v.x - p.xAxis;
                    closest.cy = v.y - p.yAxis;
                    var dist = Math.sqrt(closest.cx * closest.cx + closest.cy * closest.cy);
                    if (dist < closest.distance) {
                        closest.id = v.id;
                        closest.distance = dist;
                        closest.type = v.type;
                    }
                });
                p.object.id = closest.id;
                p.object.type = closest.type;
                //}
            });
            return true;
        })

        self.pointers = newPoints;
        self.removeDuplicatedlines();
    }
    removeDuplicatedlines() {
        var self = this;
        var list = new Array();
        var newPoints = self.pointers.filter(function (point) {
            var keyfirst = point.points[0].object.id + "" + point.points[0].object.type;
            var keylast = point.points[1].object.id + "" + point.points[1].object.type;
            if ((list.indexOf(keyfirst + "" + keylast) !== -1) || (list.indexOf(keylast + "" + keyfirst) !== -1) || keyfirst === keylast) {
                return false;
            } else {
                list.push(keyfirst + "" + keylast);
                return true;
            }
        })
        self.pointers = newPoints;
    }
}
var wf = stage.addElement(new WF({ id: "wf" })).render().getElement('wf');
jQuery(function () {
    var container = $(".workflow-viewport-canvas")
    $(wf.stage.canvas).css({ left: (container.width() / 2 - wf.stage.width / 2), top: (container.height() / 2 - wf.stage.height / 2) })
    $(".workflow-viewport-canvas").animate({ opacity: 1 }, 200);
    $("#wf-ui-dragable").sortable({
        revert: false,
        handle: ".drag-icon"
    }).disableSelection();
    $("#wf-ui-dragable li,#wfs-ui-dragable li").draggable({
        helper: "clone",
        cancel: false,
        connectToSortable: "#workflow-vp",
        handle: ".drag-icon"
    }).disableSelection();


    $("#workflow-vp").droppable({
        hoverClass: 'ui-state-active',
        tolerance: 'pointer',
        accept: function (event, ui) {
            return true;
        },
        drop: function (event, ui) {
            if (wf.vars.formId === null) {
                $('#empty-stage').modal('show');
            } else {
                

                var ob = $(ui.helper).data("json");
                ob.x = ui.offset.left - $(this).offset().left;
                ob.y = ui.offset.top - $(this).offset().top;
                console.log(ob);
                if (ob.type == "start-state") {
                    var found = false;
                    if (wf.objects.length > 0) {
                        for (j = 0; j < wf.objects.length; j++) {
                            if (wf.objects[j].type == ob.type) {
                                toastr.info("Only one start can exist.", "Element Workflow");
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found) {
                        createElementState("start", function (data) {
                            Object.keys(data).forEach(function (i) {
                                ob[i] = data[i];
                            });
                            wf.insertObject(ob);
                        });
                    }
                }
                else if (ob.type == "end-state") {
                    createElementState("end", function (data) {
                        Object.keys(data).forEach(function (i) {
                            ob[i] = data[i];
                        });
                        wf.insertObject(ob);
                    });

                }
                else {
                    $('#wf-' + ob.type).removeAttr("data-json").attr("data-json", JSON.stringify(ob));
                    $('#wf-' + ob.type).modal('show');
                }
               
               
            }
            $(ui.helper).removeAttr("style").show();
        }
    });
    $('#dropdownMenuButton + [aria-labelledby="dropdownMenuButton"] a').on('click', function (e) {
        e.preventDefault();
        $(this).closest(".dropdown").find("#dropdownMenuButton").html($(this).html());
        wf.vars.addStateForForm = "";
        wf.vars.removeStateForForm = "";
        wf.vars.formId = $(this).data("id");
        
    });
    var m = { "d": false, "e": null };
    $('#workflow-vp').on('mousedown', function (e) {
        if (!stage.isValid) {
            stage.canvas.style.cursor = stage.cursors.grabbing();
            m.d = true;
            m.e = { x: stage.canvas.offsetLeft - e.clientX, y: stage.canvas.offsetTop - e.clientY };
        }
    }).on('mousemove', function (e) {
        if (!stage.isValid) {
            stage.canvas.style.cursor = stage.cursors.grab();
            if (m.d) {
                var l = (e.clientX + m.e.x);
                var t = (e.clientY + m.e.y);
                if (l <= 0) {
                    stage.canvas.style.left = l + "px";
                }
                if (t <= 0) {
                    stage.canvas.style.top = t + "px";
                }


            }
        }

    }).on('mouseup', function (e) {
        if (!stage.isValid) {
            stage.canvas.style.cursor = stage.cursors.grab();
            m.d = false;
        }

    });
    $(document).on("click", "[data-insert]", function () {
        var check = $(this).parents(".modal").attr("id");
        var ob = JSON.parse($(this).closest(".modal").attr('data-json'));
        var e = $(this).closest(".modal-content").find("input[name='insert']:checked");
        if (check === "wf-email") {
            createMailMergeObject(JSON.parse(e.attr('data-json')), function (data) {
                    Object.keys(data).forEach(function (i) {
                        ob[i] = data[i];
                    });
                    wf.insertObject(ob);
                $(this).closest(".modal").removeAttr('data-json');
               
            });
        }
        else if (check === "wf-state")
        {
            if (e) {
                e = JSON.parse(e.attr('data-json'));
                Object.keys(e).forEach(function (i) {
                    ob[i] = e[i];
                });
                wf.vars.addStateForForm = wf.vars.addStateForForm + "," + e.id;
                wf.insertObject(ob);
                $(this).closest(".modal").removeAttr('data-json');
            }
        }
        else {
            if (e) {
                e = JSON.parse(e.attr('data-json'));
                Object.keys(e).forEach(function (i) {
                    ob[i] = e[i];
                });
                wf.insertObject(ob);
                $(this).closest(".modal").removeAttr('data-json');
            }
        }
       
   
    });
    $(document).on("click", "[data-icon='move']", function () {
        $(this).removeClass('active');
        $(this).closest("button").find("[data-icon='pointer']").addClass('active');
        stage.isValid = false;
        stage.canvas.style.cursor = stage.cursors.default();
    });
    $(document).on("click", "[data-icon='pointer']", function () {
        $(this).removeClass('active');
        $(this).closest("button").find("[data-icon='move']").addClass('active');
        stage.isValid = true;
        stage.canvas.style.cursor = stage.cursors.default();
    });
    $(document).on("click", "[data-tool='delete']", function () {

        count = 0;
        count1 = 0;
        $.each(wf.objects, function (x, y) {
            if (y.active === true) { count++; }
        });
        $.each(wf.pointers, function (x, y) {
            if (y.active === true) { count++; }
        });
        if (count === 0 && count1 === 0) {
            $("#delete-warning").find(".modal-body").html("<p>"
                + "Please select and object before deleting."
                + "</p>");
        }
        else {
            $("#delete-warning").find(".modal-body").html("<p>"
                + "Are you sure? Do you want to delete Active object from workflow stage?"
                + "</p>");
        }
        $("#delete-warning").modal('show');
    });
    $(document).on("click", "#delete-warning .btn-primary", function () {
        $('[data-tool="delete"]').attr("disabled", true);
        wf.deleteActive();
        wf.tools.delete = false;
        stage.isValid = true;
        stage.canvas.style.cursor = stage.cursors.default();

    });
    //edit workflow element settings
    $(document).on("click", "[data-tool='edit']", function () {
        count = 0;
        $.each(wf.objects, function (x, y) {
            if (y.active === true) { count++; }
        });


        if (count === 1) {
            if (wf.selected !== null) {
                editObject = true;

                $("[data-save]").attr("style", "display:block;");
                switch (wf.selected.type) {
                    case "api":
                        $("#wf-api-new").modal("show");
                        setTimeout(function () {
                            getComponentByIdType(wf.selected.id, "api", false);
                            $("#wf-api-save").addClass("data-save-api1");
                            saveApiSetting();
                        }, 300);
                        break;
                    case "automation":
                        $("#wf-automation-new").modal("show");
                        setTimeout(function () {
                            getComponentByIdType(wf.selected.id, "automation", false);
                            $("#wf-automation-save").addClass("data-save-auto");
                            saveAutoSetting();
                        }, 300);
                        break;
                    case "assign":
                        $("#wf-assign-new").modal("show");
                        setTimeout(function () {
                            getComponentByIdType(wf.selected.id, "assign", false);
                            $("#wf-assign-save").addClass("data-save-assign");
                            saveAssignSetting();
                        }, 300);
                        break;
                    case "email":
                        $("#wf-email-new").modal("show");
                        setTimeout(function () {
                            getMailMergeObject(wf.selected.id);

                        }, 300);
                        break;
                    case "state":
                        editSelectedState("state", "wf-state", wf.selected.id, wf.vars.formId);
                        break;
                    case "caseassignment":
                        $("#wf-caseassignment-new").modal("show");
                        setTimeout(function () {
                            getComponentByIdType(wf.selected.id, "caseassignment", false);
                            $("#wf-caseassignment-save").addClass("data-save-caseassignment");
                            saveCaseAssignSetting();
                        }, 300);
                        break;

                }

            }
        }
        else {
            toastr.warning("Please select only one workflow element to edit.");
        }
    });
    $(document).on("click", "[data-tool='reset']", function () {
        $("#reset-warning").modal('show');
    });
    $(document).on("click", "#reset-warning .btn-primary", function () {
        $('[data-tool="delete"]').attr("disabled", true);
        wf.deleteAll();
        wf.tools.delete = false;
        stage.isValid = true;
        stage.canvas.style.cursor = stage.cursors.default();

    });
    $(document).on("click", "[data-tool='save']", function () {
        $('[data-tool="tool"]').attr("disabled", true);
        wf.tools.delete = false;
        wf.tools.save = false;
        stage.isValid = true;
        stage.canvas.style.cursor = stage.cursors.default();
        var form_id = wf.vars.formId;
        if (form_id === "" || $.active > 0) return;
        $.ajax({
            url: "/admin/manage/elementworkflow",
            cache: false,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            //      data: { "tenant_identifier": WF_Vars.id, "form_id": form_id, "allPoints": JSON.stringify(wf.export()), Type: "state" },
            data: JSON.stringify({
                "tenant_identifier": WF_Vars.id,
                "form_id": form_id, "removeStateForForm": wf.vars.deleteStateForForm, "addStateForForm": wf.vars.addStateForForm, "allPoints": wf.export(),
                "elementId": WF_Vars.elementId, "eventType": wf.vars.eventType
            }),
            success: function (html) {
                if (html !== null && html !== "" && html !== " ") {
                    let message = html.message;
                    if (html.status ==="error") {
                        toastr.error(message,"Save unsuccessfull.");
                    }
                    else {
                        toastr.success(message);
                    }
                    //toastr.error(message);
                    //$(".status").replaceWith('<div class="status show"><div class="alert alert-' + html.status + ' alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' + message + '</div></div>');
                }

            }
        });


    });
    $(document).on("click", "[data-form-id]", function () {
        $('[data-tool="tool"]').attr("disabled", true);
        wf.tools.delete = false;
        wf.tools.save = false;
        stage.isValid = true;
        stage.canvas.style.cursor = stage.cursors.default();
        var form_id = wf.vars.formId;
        wf.vars.elementId = WF_Vars.elementId;
        wf.vars.eventType = WF_Vars.eventType;
        if (form_id === "" || $.active > 0) return;
        $.ajax({
            url: "/admin/manage/pullelementworkflow",
            cache: false,
            type: "POST",
            dataType: "json",
            data: { "tenantIdentifier": WF_Vars.id, "formId": form_id, "elementId": WF_Vars.elementId, "eventType": WF_Vars.eventType },
            success: function (html) {
                wf.objects = [];
                wf.pointers = [];
                try {
                    for (i = 0; i < html.json.object.length; i++) {
                        var o = html.json.object[i];
                        o.stage = stage;
                        wf.pushObject(o);
                    }
                    for (j = 0; j < html.json.point.length; j++) {
                        var p = html.json.point[j];
                        p.stage = stage;
                        wf.pushPoint(p);

                    }
                }
                catch (e) {}

            }
        });


    });
})