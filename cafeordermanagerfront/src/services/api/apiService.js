import axios from "axios";
import store from "store";
import general from "../../utils/general";

const makeApiCall = async payload => {
    try {
        if (payload.query) {
            payload.query = general.removeNullValues(payload.query);
        }
        const sendData = general.isNullOrEmpty(payload?.data) ? {} : { ...payload.data };
        let url = process.env.REACT_APP_API_URL + "/" + payload.url;
        if (payload?.query instanceof Object) {
            const keys = Object.keys(payload?.query);

            if (keys?.length > 0) {
                // Son karakterin & olup olmadığını kontrol ediyoruz
                if (url.endsWith('&') || url.endsWith('?')) {
                    url = url.slice(0, -1);  // Son karakteri kaldırır
                }
                // Yeni query parametreleri eklemeden önce ? işaretini kontrol ederiz
                if (!url.includes('?')) {
                    url += "?";
                }
            }

            let query = "";
            for (let i = 0; i < keys.length; ++i) {
                const object = payload?.objectKeys?.find(item => item == keys[i]) != null;

                query += keys[i] + "=" + encodeURI(
                    object
                        ? JSON.stringify(payload?.query[keys[i]])
                        : (payload?.query[keys[i]] instanceof Array
                            ? payload?.query[keys[i]].join(",")
                            : payload?.query[keys[i]])
                );

                if (i < keys.length - 1) {
                    query += "&";
                }
            }

            // Eğer query kısmında eklenen bir şey varsa, URL'nin sonuna ekleriz
            if (query) {
                url += (url.endsWith('?') ? '' : '&') + query;
            }

            if (payload?.saveFilter) {
                const base_url = window.location.href.split("?")[0];
                window.history.pushState({}, null, base_url + "?" + query);
            }
        }
        const token = store.get("token");
        const newToken = general.isNullOrEmpty(payload.token) ? token : payload.token;
        let options = {
            url,
            headers: {
                "Accept": 'application/json',
                "Content-Type": "application/json",
                "Authorization": "Bearer " + newToken,
                'Access-Control-Allow-Origin': '*'
            },
            method: payload.method,
            data: sendData,
            ...payload?.axiosOptions
        };
        const res = await axios(options);
        if (res.status === 200 || res.status === 201 || res.status === 204) {
            return {
                success: true,
                data: res.data?.data,
                pagination: res.data?.pagination,
            }
        }
        return {
            error: true,
            data: res.data?.data,
            errorMessage: res.data?.exceptionMessage,
            errorMessageTechnical: res.data?.exceptionMessageTechnical,
        }
    } catch (error) {
        const statusCodeString = error?.response?.status + "";
        if (error?.response?.status == 401) {
            general.notificationError("error.unauthorized");
            setTimeout(() => {
                store.remove("token");
                window.location.href = "/auth/login"
            }, 2000);
            return {}
        }
        if (statusCodeString.startsWith("4")) {
            console.log("error", error?.response?.data?.exceptionMessage);
            return {
                error: true,
                errorMessage: error?.response?.data?.exceptionMessage,
            }

        }
        if (error?.response?.status == 500) {
            return {
                error: true,
                errorMessage: error?.response?.data?.errorCode,
            }
        }
        else {
            return {
                error: true,
                data: null,
                errorMessage: "Sunucu ile bağlantı kurulamadı/Server connection error",
                errorMessageTechnical: error?.message,
                useI18next: false
            }
        }
    }
};


export default {
    makeApiCall
}