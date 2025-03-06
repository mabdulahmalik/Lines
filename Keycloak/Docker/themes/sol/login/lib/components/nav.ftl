<#import "../components.ftl" as components>

<#macro nav>
    <nav class="px-4 py-2">
        <div class="m-0 p-0">
<#--            <a class="" href="#">-->
<#--                <@components.logo />-->
<#--            </a>-->

            <div>
                <#if realm.internationalizationEnabled  && locale.supported?size gt 1>
                    <div class="flex flex-col items-end">
                        <button onclick="toggleDropdownMenu()" id="langSelectButton" class="inline-flex items-center space-x-2 px-4 py-1 rounded-md" type="button">
                            <svg width="15" height="15" viewBox="0 0 15 15" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <g clip-path="url(#clip0_2209_170)">
                                    <path d="M7.5 0C3.3645 0 0 3.3645 0 7.5C0 11.6355 3.3645 15 7.5 15C11.6355 15 15 11.6355 15 7.5C15 3.3645 11.6355 0 7.5 0ZM13.4482 6.75H11.3752C11.2827 5.10843 10.8235 3.50847 10.0312 2.06775C10.9494 2.49679 11.7441 3.15105 12.3416 3.96968C12.939 4.78832 13.3197 5.74474 13.4482 6.75ZM7.8975 1.52025C8.67375 2.54325 9.71775 4.35525 9.86775 6.75H5.2725C5.37675 4.803 6.018 2.979 7.11075 1.5195C7.23975 1.512 7.36875 1.5 7.5 1.5C7.63425 1.5 7.7655 1.512 7.8975 1.52025ZM5.016 2.04525C4.278 3.4635 3.852 5.0715 3.7725 6.75H1.55175C1.68133 5.73588 2.06748 4.77158 2.67367 3.94831C3.27986 3.12505 4.08606 2.47004 5.016 2.04525ZM1.55175 8.25H3.78225C3.88425 10.0343 4.281 11.6085 4.94925 12.9225C4.03589 12.492 3.2458 11.8381 2.65197 11.0215C2.05814 10.2048 1.67975 9.2516 1.55175 8.25ZM7.0875 13.4797C6.03675 12.2062 5.4165 10.422 5.28075 8.25H9.8655C9.7095 10.3298 9.02775 12.147 7.91325 13.479C7.77675 13.488 7.64025 13.5 7.5 13.5C7.3605 13.5 7.22475 13.488 7.0875 13.4797ZM10.0958 12.9008C10.812 11.5553 11.2493 9.975 11.364 8.25H13.4475C13.3209 9.24321 12.948 10.189 12.3626 11.0013C11.7772 11.8135 10.9979 12.4665 10.0958 12.9008Z" fill="black"/>
                                </g>
                                <defs>
                                    <clipPath id="clip0_2209_170">
                                        <rect width="15" height="15" fill="white"/>
                                    </clipPath>
                                </defs>
                            </svg>

                            <#--                                                        ${locale.currentLanguageTag?upper_case}-->
                            <span>
                                    ${locale.current}
                                </span>
                            <svg width="15" height="15" viewBox="0 0 15 15" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <g clip-path="url(#clip0_2209_174)">
                                    <path d="M1.67 4.28653C1.28796 3.90449 0.66856 3.90449 0.286526 4.28653C-0.0955086 4.66856 -0.0955086 5.28796 0.286526 5.67L6.80826 12.1917C7.1903 12.5738 7.8097 12.5738 8.19173 12.1917L14.7135 5.67C15.0955 5.28796 15.0955 4.66856 14.7135 4.28653C14.3314 3.90449 13.712 3.90449 13.33 4.28653L7.5 10.1165L1.67 4.28653Z" fill="black"/>
                                </g>
                                <defs>
                                    <clipPath id="clip0_2209_174">
                                        <rect width="15" height="15" fill="white"/>
                                    </clipPath>
                                </defs>
                            </svg>
                        </button>

                        <!-- Dropdown menu -->
                        <div onmouseleave="toggleDropdownMenu()" id="langSelectDropdown" class="z-10 hidden w-54 fixed my-8 bg-white border rounded-md">
                            <ul class="">
                                <#list locale.supported as l>
                                    <#if locale.current != l.label>
                                        <li class="px-4 py-1 rounded-md">
                                            <a class="no-underline text-black font-thin hover:text-primary" href="${l.url}">
                                                <#--                                                ${l.languageTag?upper_case}-->
                                                ${l.label}
                                            </a>
                                        </li>
                                    </#if>
                                </#list>
                            </ul>
                        </div>
                    </div>
                    <script type="text/javascript">
                        function toggleDropdownMenu() {
                            document.getElementById("langSelectDropdown").classList.toggle("hidden");
                            document.getElementById("langSelectButton").classList.toggle("bg-[#E5EBFF]");
                        }
                    </script>
                </#if>
            </div>
        </div>
    </nav>
</#macro>