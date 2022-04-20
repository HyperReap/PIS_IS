export default {    
    install(app, options) {
        app.config.globalProperties.$createRequestParams = function (object, isFirstArrayOrObject) {
            let param = "?";
            Object.keys(object).forEach(function (key, i) {
                //pokud se jedna o objekt nebo pole, tak to chci posilat az v body
                //takovy je vzdy na prvni pozici
                if (i === 0 && isFirstArrayOrObject) {
                    return;
                }
                //nechci posilat null
                if (object[key] === null) {
                    return;
                }
                param += (param == "?") ? key + "=" + object[key] : "&" + key + "=" + object[key];
            });
            return (param == "?" ? "" : param);
        }
        app.config.globalProperties.$createBodyParams = function (object) {
            let name = Object.keys(object)[0];
            let values = Object.values(object)[0];
            let list = [];
            values.forEach(element => {
                list.push({"name": element})
            });
            return { [name]: list };
        }
    }
};