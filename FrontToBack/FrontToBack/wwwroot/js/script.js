$(document).ready(function () {

    let skip = 4
    let scroll = 500
    let bool = true
    $(window).scroll(function () {
        if ($(window).scrollTop() >= scroll && bool) {
            console.log("w");
            scroll += 350;
            $.ajax({
                type: "GET",
                url: "/Home/Load?skip=" + skip,
                success: function (res) {

                    $("#productsRow").append(res);
                    skip += 4;
                    let productsCount = $("#productsCount").val();
                    if (skip >= productsCount) {
                        bool = false
                    }
                }
            });
        }
    })
   
    $(document).on('keyup', '#input-search', function () {
        let searchedProduct = $(this).val()
        $("#searched li").slice(1).remove()
        $.ajax({
            type: "GET",
            url: "/Home/Search?searcedProduct=" + searchedProduct,
            success: function (res) {
                $("#searched").append(res)
            }
        })
    })
    $(document).on('click', '#addToBasket', function () {
        let productId = $(this).attr("data-id")
        $.ajax({
            type: "GET",
            url: "/Home/AddToBasket?id=" + productId,
            success: function (res) {
                $("#basketCount").text(res[1])
                document.querySelector("#totalCart").innerText = res[0];
            }
        })
    })
    $(document).on('click', '.changeCount', function () {
        let productId = $(this).attr("data-id")
        let operationType = $(this).attr("data-operation")
        $.ajax({
            type: "GET",
            url: "/Basket/ChangeCount",
            data: {
                id: productId,
                operation: operationType
            },
            success: function (res) {
                $("#basketBody").remove();
                $("#basketTable").append(res);
                let count = parseInt($("#basketBody tr").last().attr("data-id")) + 1;
                let totalCart = 0;
                isNaN(count) ? count = 0 : "";
                document.querySelectorAll("#basketBody tr").forEach((tr) => {
                    totalCart += tr.querySelector("#price").innerText * tr.querySelector("#count").innerText;
                })
                document.querySelector("#basketCount").innerText = count;
                document.querySelector("#totalCart").innerText = totalCart;
            }
        })
    })
    $(document).on('click', '#deleteProductFromBasket', function () {
        let productId = $(this).attr("data-id")
        $.ajax({
            type: "GET",
            url: "/Basket/DeleteProductFromBasket?id=" + productId,
            success: function (res) {
                $("#basketBody").remove();
                $("#basketTable").append(res);
                let count = parseInt($("#basketBody tr").last().attr("data-id")) + 1;
                let totalCart = 0;
                document.querySelectorAll("#basketBody tr").forEach((tr) => {
                    totalCart += tr.querySelector("#price").innerText * tr.querySelector("#count").innerText;
                })
                isNaN(count) ? count = 0 : "";
                document.querySelector("#basketCount").innerText = Number(count);
                document.querySelector("#totalCart").innerText = totalCart;
            }
        })
    })
    // HEADER

    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })
    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})