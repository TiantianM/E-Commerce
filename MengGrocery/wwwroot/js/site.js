// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
  if (document.location.href.toLowerCase().includes("product")) {
    // category filter
    let catBtns = document.querySelectorAll(".cat-btn");
    catBtns.forEach(function (btn) {
      btn.addEventListener("click", function () {
        var category = btn.getAttribute("category");
        if (category && category.length > 0) {
          if (category === "All") {
            var productItems = document.querySelectorAll(".product-item");
            productItems.forEach(function (item) {
              item.style.display = "block";
            });
            return;
          }
          var productItems = document.querySelectorAll(".product-item");
          productItems.forEach(function (item) {
            var productCategory = item.querySelector(
              'input[name="category"]'
            ).value;
            if (productCategory != category) {
              item.style.display = "none";
            } else {
              item.style.display = "block";
            }
          });
        }
      });
    });

    // auto search
    let searchInput = document.querySelector(".search-input");
    let productItems = document.querySelectorAll(".product-item");
    searchInput.addEventListener("keyup", function () {
      var searchValue = searchInput.value;
      if (searchValue && searchValue.length > 0) {
        productItems.forEach(function (item) {
          let productName = item.querySelector(".product-name").innerText;
          if (
            productName.toLowerCase().indexOf(searchValue.toLowerCase()) === -1
          ) {
            item.style.display = "none";
          } else {
            item.style.display = "block";
          }
        });
      } else {
        productItems.forEach(function (item) {
          item.style.display = "block";
        });
      }
    });
  }

  //after click remove button <a> in cart page, stop default behavior, make ajax call to get updated cart partial view and replace
  // let removeBtns = document.querySelectorAll(".remove-cart-item");
  // removeBtns.forEach(function (btn) {
  //   btn.addEventListener("click", function (e) {
  //     e.preventDefault();
  //     //let url = btn.getAttribute("href").replace("RemoveItem", "AjaxRemove");
  //     let url = btn.getAttribute("href").replace("RemoveItem", "AjaxRemove");
  //     fetch(url)
  //       .then((response) => response.text())
  //       .then((data) => {
  //         let cartContainer = document.querySelector(".cart-list");
  //         cartContainer.innerHTML = data;
  //       })
  //       //then update cart item count
  //       .then(() => updateCartItemCount());
  //   });
  // });

  //update cart item count at cart icon
  let cartItemCount = document.querySelector(".nav-cart .item-count");
  if (cartItemCount) {
    updateCartItemCount();
  }

  function updateCartItemCount() {
    fetch("/cart/GetCartItemCount")
      .then((response) => response.json())
      .then((data) => {
        cartItemCount.innerText = data;
      });
  }

  // let quantityInputs = document.querySelectorAll(".update-cart-item");
  // //update with json sample code
  // quantityInputs.forEach(function (input) {
  //   input.addEventListener("click", function (e) {
  //     e.preventDefault();
  //     let form = input.closest("form");
  //     let jsonBody = {
  //       ProductId: form.querySelector('input[name="ProductId"]').value,
  //       Quantity: form.querySelector('input[name="Quantity"]').value,
  //     };
  //     let url = form
  //       .getAttribute("action")
  //       .replace("UpdateItem", "AjaxUpdateQuantityJson");
  //     fetch(url, {
  //       method: "POST",
  //       headers: {
  //         "Content-Type": "application/json",
  //       },
  //       body: JSON.stringify(jsonBody),
  //     })
  //       .then((response) => response.text())
  //       .then((data) => {
  //         var jsonResult = JSON.parse(data);
  //         if (!jsonResult.success) {
  //           alert("update failed");
  //         } else {
  //           updateCartItemCount();
  //         }
  //       });
  //   });
  // });

  // lazy load images

  var lazyImages = [].slice.call(document.querySelectorAll("img.lazy-load"));

  if ("IntersectionObserver" in window) {
    let lazyImageObserver = new IntersectionObserver(function (
      entries,
      observer
    ) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          let lazyImage = entry.target;
          //make sure image has data-src attribute
          if (!lazyImage.dataset.src) {
            return;
          }
          lazyImage.src = lazyImage.dataset.src;
          lazyImage.classList.remove("lazy-load");
          lazyImageObserver.unobserve(lazyImage);
        }
      });
    });

    lazyImages.forEach(function (lazyImage) {
      lazyImageObserver.observe(lazyImage);
    });
  } else {
    // Fallback for browsers that don't support IntersectionObserver
    lazyImages.forEach(function (lazyImage) {
      if (!lazyImage.dataset.src) {
        return;
      }
      lazyImage.src = lazyImage.dataset.src;
    });
  }

  let sameAsShipping = document.querySelector("#BillingSameAsShipping");
  if (sameAsShipping) {
    sameAsShipping.addEventListener("change", function () {
      let billingSection = document.querySelector(".billing-section");
      let inputs = billingSection.querySelectorAll("input");
      if (sameAsShipping.checked) {
        inputs.forEach(function (input) {
          input.disabled = true;
        });
      } else {
        inputs.forEach(function (input) {
          input.disabled = false;
        });
      }
    });
  }
});
