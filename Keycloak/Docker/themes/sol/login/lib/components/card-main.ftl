<#import "../components.ftl" as components>

<#macro cardMain header="" hasBackToLoginLink=false displayMessage=true>
    <div class="flex flex-col mx-[2rem]">
        <#if hasBackToLoginLink>
            <div class="my-4">
                <a href="${url.loginRestartFlowUrl}" class="flex space-x-2 no-underline">
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M13.8974 4.89742L12 3L3 12L12 21L13.8974 19.1026L8.13672 13.3419L21 13.3419L21 10.6581L8.13672 10.6581L13.8974 4.89742Z" fill="#282C34"/>
                    </svg>

                    <div class="text-black">
                        ${kcSanitize(msg("back"))?no_esc}
                    </div>
                </a>
            </div>
        </#if>
        <div>
            <#if header?has_content>
                <h1 class="text-2xl md:text-2xl text-primary font-semibold">
                    ${header}
                </h1>
            </#if>
        </div>

        <div class="my-4">
            <@components.alerts displayMessage=displayMessage />
        </div>

        <div class="space-y-5">
            <#nested>
        </div>
    </div>
</#macro>