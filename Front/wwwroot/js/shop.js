import { toggleLoader } from './loaderHelper.js';
import { API_BASE_URL } from './apiConfig.js';

$(document).ready(function () {
    $.ajax({
        url: `${API_BASE_URL}/api/Category/GetCategories`,
        method: "GET",
        dataType: "json",
        success: function (categories) {
            console.log(categories)
            renderCategories(categories);

            if (categories.length > 0) {
                loadProducts(categories[0].id);
            }
        },
        error: function (xhr, status, error) {
            console.error("Hata:", error);
        },
        complete: function (xhr, status) {
            toggleLoader();
        }
    });

    function renderCategories(categories) {
        const accordionContainer = document.getElementById("accordionExample");
        accordionContainer.innerHTML = "";
        let selectedCategoryId = categories[0]?.id || null;

        categories.forEach((category, index) => {
            const collapseId = `collapse${category.id}`;

            // Kategori kartını oluştur
            const categoryCard = document.createElement("div");
            categoryCard.classList.add("card");

            // Başlık kısmı
            const cardHeading = document.createElement("div");
            cardHeading.classList.add("card-heading");

            const headingLink = document.createElement("a");
            headingLink.setAttribute("data-toggle", "collapse");
            headingLink.setAttribute("data-target", `#${collapseId}`);
            headingLink.setAttribute("data-id", category.id);
            headingLink.classList.add("category-link");
            headingLink.textContent = category.name;

            if (category.id == selectedCategoryId) {
                cardHeading.classList.add("active");
                headingLink.classList.add("active");
            }

            cardHeading.appendChild(headingLink);

            
            const collapseDiv = document.createElement("div");
            collapseDiv.id = collapseId;
            collapseDiv.classList.add("collapse");

            if (index === 0) {
                collapseDiv.classList.add("show");
            }

            collapseDiv.setAttribute("data-parent", "#accordionExample");

            const cardBody = document.createElement("div");
            cardBody.classList.add("card-body");

            const ul = document.createElement("ul");

            if (category.childCategories) {
                category.childCategories.forEach(subCategory => {
                    const li = document.createElement("li");
                    const link = document.createElement("a");
                    link.href = `#`;
                    link.setAttribute("data-id", subCategory.id);
                    link.classList.add("category-link");
                    link.textContent = subCategory.name;

                    if (subCategory.id == selectedCategoryId) {
                        link.classList.add("active");
                        cardHeading.classList.add("active");
                        collapseDiv.classList.add("show");
                    }

                    li.appendChild(link);
                    ul.appendChild(li);
                });
            }

            cardBody.appendChild(ul);
            collapseDiv.appendChild(cardBody);
            categoryCard.appendChild(cardHeading);
            categoryCard.appendChild(collapseDiv);
            accordionContainer.appendChild(categoryCard);
        });

        
        document.querySelectorAll(".category-link").forEach(link => {
            link.addEventListener("click", function (event) {
                event.preventDefault(); 

                const categoryId = this.getAttribute("data-id");

                if (selectedCategoryId !== categoryId) {
                    selectedCategoryId = categoryId; 
                    loadProducts(categoryId);
                } else {
                    console.log(`Kategori ${categoryId} zaten seçili, istek yapılmadı.`);
                }
            });
        });
    }

    function loadProducts(categoryId) {
        toggleLoader();
        $.ajax({
            url: `${API_BASE_URL}/api/Product/GetProductsByCategory/${categoryId}`,
            method: "GET",
            dataType: "json",
            success: function (products) {
                console.log(`Kategori ${categoryId} için ürünler:`, products);
                renderProducts(products); 
            },
            error: function (xhr, status, error) {
                console.error("Ürünleri yüklerken hata:", error);
            },
            complete: function (xhr, status) {
                toggleLoader();
            }
        });
    }

    function renderProducts(products) {
        const productContainer = document.getElementById("productContainer");
        productContainer.innerHTML = "";

        products.forEach(product => {
            const productHTML = `
                <div class="col-lg-4 col-md-6">
                    <div class="product__item">
                        <div class="product__item__pic set-bg" style="background-image: url('${API_BASE_URL}${product.imageUrl}');">
                            <div class="label new">${product.isNew ? "New" : ""}</div>
                            <ul class="product__hover">
                                <li><a href="#">
                                    <span class="image-popup product_button" data-href="${API_BASE_URL}${product.imageUrl}" style="width:100%">
                                        <span class="arrow_expand"></span>
                                    </span></a>
                                </li>
                                <li><a href="#"><span class="icon_heart_alt"></span></a></li>
                                <li><a href="#"><span class="icon_bag_alt"></span></a></li>
                            </ul>
                        </div>
                        <div class="product__item__text">
                            <h6><a href="#">${product.name}</a></h6>
                            <div class="rating">
                                ${[...Array(5)].map((_, i) => `<i class="fa ${i < product.rating ? 'fa-star' : 'fa-star-o'}"></i>`).join(" ")}
                            </div>
                            <div class="product__price">$${product.price}</div>
                        </div>
                    </div>
                </div>
            `;

            productContainer.innerHTML += productHTML;
        });
    }

    $(document).on("click", ".image-popup", function (event) {
        event.preventDefault();
        const imageUrl = $(this).attr("data-href");

        $.magnificPopup.open({
            items: {
                src: imageUrl
            },
            type: "image"
        });
    });

});
