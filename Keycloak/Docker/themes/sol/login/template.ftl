<#import "lib/components.ftl" as components>

<#macro registrationLayout title=realm.displayName!'' displayMessage=true imgRight="signin">
<!DOCTYPE html>
<html <#if realm.internationalizationEnabled> lang="${locale.currentLanguageTag}"</#if>>

<@components.head title=title/>

<body class="grid grid-cols-12 gap-0 justify-between font-plusJakartaSans ${properties.theme!'light'}">

    <main class="col-span-12 md:col-span-5 space-y-10 my-5 px-2 md:px-4">

        <@components.nav/>

        <div class="flex justify-center">
            <@components.logo/>
        </div>

        <#nested "form">

<#--        <@components.footer/>-->
    </main>

    <#switch imgRight>
        <#case "signin">
            <div class="sticky top-0 hidden md:block col-span-12 md:col-span-7 h-screen bg-[url('../img/img-right-signin.png')] bg-cover bg-center"></div>
        <#break>
        <#case "forgot-password">
            <div class="sticky top-0 hidden md:block col-span-12 md:col-span-7 h-screen bg-[url('../img/img-right-forgot-password.png')] bg-cover bg-center"></div>
        <#break>
        <#case "create-account">
            <div class="sticky top-0 hidden md:block col-span-12 md:col-span-7 h-screen bg-[url('../img/img-right-create-account.png')] bg-cover bg-center"></div>
        <#break>
        <#case "choose-org">
            <div class="sticky top-0 hidden md:block col-span-12 md:col-span-7 h-screen bg-[url('../img/img-right-create-account.png')] bg-cover bg-center"></div>
        <#break>
        <#default>
            <div class="sticky top-0 hidden md:block col-span-12 md:col-span-7 h-screen bg-[url('../img/img-right-signin.png')] bg-cover bg-center"></div>
    </#switch>
</body>
</html>
</#macro>
