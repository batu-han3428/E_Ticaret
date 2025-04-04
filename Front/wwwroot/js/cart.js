import { apiRequest } from './apiRequest.js';
import { API_BASE_URL } from './apiConfig.js';

export async function getCart() {
    try {
        const cart = await apiRequest(`${API_BASE_URL}/api/Cart/GetCart`);
        console.log("Sepet Verisi:", cart);
/*        document.removeEventListener('csrfTokenReady', onCsrfTokenReady);*/

        updateCartCount(cart.cartItems.reduce((total, item) => total + item.quantity, 0));
    } catch (error) {
        console.error("Hata:", error);
    }
}

function updateCartCount(count) {
    const cartElements = document.querySelectorAll(".cartItemCount");
    cartElements.forEach(element => {
        element.textContent = count;
    });
}

//await getCart();
document.addEventListener('DOMContentLoaded', getCart);

//document.addEventListener('csrfTokenReady', async () => {
//    await getCart();
//});
