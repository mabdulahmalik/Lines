<#macro otpInput inputId btnId>
    <style>
        input:focus{
            outline: none;
        }
    </style>
    <div id="otp" class="flex justify-between text-2xl" dir="ltr" >
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="first" autocomplete="off" placeholder="" autofocus />
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="second" autocomplete="off" placeholder="" onclick="focusFirst('first')" />
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="third" autocomplete="off" placeholder="" onclick="focusFirst('first')" />
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="fourth" autocomplete="off" placeholder="" onclick="focusFirst('first')" />
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="fifth" autocomplete="off" placeholder="" onclick="focusFirst('first')" />
        <input onkeyup="concatenateOtp(); enableNext()" style="width: 3rem; height: 3rem;" class="rounded-circle border text-center" type="text" maxlength="1" pattern="\d" id="sixth" autocomplete="off" placeholder="" onclick="focusFirst('first')" />
    </div>
    <script>
        function focusFirst(firstDigitInputId) {
            document.getElementById(firstDigitInputId).focus();
        }

        function OTPInput() {
            const inputs = document.querySelectorAll('#otp > *[id]');
            for (let i = 0; i < inputs.length; i++) {
                inputs[i].addEventListener('keydown', function(event) {
                    if (event.key === "Backspace") {
                        inputs[i].value = '';
                        if (i !== 0)
                            inputs[i - 1].focus();
                    } else {
                        if (i === inputs.length - 1 && inputs[i].value !== '') {
                            return true;
                        } else if ((event.keyCode > 47 && event.keyCode < 58)
                            || (event.keyCode > 95 && event.keyCode < 106)){
                            inputs[i].value = event.key;
                            if (i !== inputs.length - 1)
                                inputs[i + 1].focus();
                            event.preventDefault();
                        } else {
                            inputs[i].value = null;
                            inputs[i].focus();
                            event.preventDefault();
                        }
                    }
                });
            }
        }
        OTPInput();

        function concatenateOtp() {
            let firstDigit = document.getElementById("first").value;
            let secondDigit = document.getElementById("second").value;
            let thirdDigit = document.getElementById("third").value;
            let fourthDigit = document.getElementById("fourth").value;
            let fifthDigit = document.getElementById("fifth").value;
            let sixthDigit = document.getElementById("sixth").value;

            document.getElementById('${inputId}').setAttribute("value", firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit + sixthDigit);
        }

        function enableNext() {
            const input = document.getElementById('${inputId}');
            const btn = document.getElementById('${btnId}');
            if (input.value.length === 6) {
                btn.removeAttribute("disabled");
                btn.classList.remove("btn-secondary");
                btn.classList.add("btn-primary");
            }else {
                btn.setAttribute("disabled", "disabled");
                btn.classList.add("btn-secondary");
                btn.classList.remove("btn-primary");
            }
        }
    </script>
</#macro>