/*import { getCart } from './cart.js';*/
import { getCookie } from './cookieHelper.js';
import { API_BASE_URL } from './apiConfig.js';

function fetchCsrfAndApiKey() {
    return fetch(`${API_BASE_URL}/api/Security/csrf-token`, {
        method: "GET",
        credentials: "include",
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("CSRF Token alınamadı.");
            }
            console.log("✅ Yeni CSRF Token ve API Key alındı.");
        })
        .catch(error => {
            console.error("❌ Hata:", error);
        });
}


export async function ensureCsrfToken() {
    let csrfToken = getCookie("CSRF-TOKEN");
    console.log(csrfToken);
    if (!csrfToken) {
        console.log("CSRF Token bulunamadı, yeniden alınıyor...");
        await fetchCsrfAndApiKey();
        csrfToken = getCookie("CSRF-TOKEN");
        if (!csrfToken) {
            throw new Error("CSRF Token alınamadı, istek iptal edildi.");
        }
    }
}



//document.addEventListener("DOMContentLoaded", fetchCsrfAndApiKey);


//window.addEventListener("popstate", fetchCsrfAndApiKey);


//document.addEventListener('csrfTokenReady', async () => {
//    await getCart();
//});
