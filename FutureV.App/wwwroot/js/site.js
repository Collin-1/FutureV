// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-detail-carousel]").forEach((carousel) => {
    const mainImage = carousel.parentElement?.querySelector(
      "[data-detail-carousel-main]",
    );
    const caption = carousel.parentElement?.querySelector(
      "[data-detail-carousel-caption]",
    );
    const thumbButtons = carousel.querySelectorAll(
      "[data-detail-carousel-thumb]",
    );

    if (!mainImage || thumbButtons.length === 0) {
      return;
    }

    const setActiveThumb = (activeButton) => {
      thumbButtons.forEach((button) => {
        const isActive = button === activeButton;
        button.classList.toggle("is-active", isActive);
        button.setAttribute("aria-pressed", isActive ? "true" : "false");
      });
    };

    thumbButtons.forEach((button) => {
      button.addEventListener("click", () => {
        const imageSrc = button.getAttribute("data-image-src");
        if (!imageSrc) {
          return;
        }

        mainImage.setAttribute("src", imageSrc);
        const imageAlt = button.getAttribute("data-image-alt");
        if (imageAlt) {
          mainImage.setAttribute("alt", imageAlt);
        }

        if (caption) {
          const imageCaption = button.getAttribute("data-image-caption");
          if (imageCaption) {
            caption.textContent = imageCaption;
          }
        }

        setActiveThumb(button);
      });
    });

    const initiallyActive = carousel.querySelector("[aria-pressed='true']");
    if (initiallyActive) {
      setActiveThumb(initiallyActive);
    }
  });
});
