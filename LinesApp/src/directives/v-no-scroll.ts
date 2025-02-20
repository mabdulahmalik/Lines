// src/directives/v-no-scroll.ts
import { DirectiveBinding } from 'vue';

const NoScrollDirective = {
  mounted(binding: DirectiveBinding) {
    if (binding.value) {
      document.body.classList.add('overflow-hidden');
    } else {
      document.body.classList.remove('overflow-hidden');
    }
  },
  updated(binding: DirectiveBinding) {
    if (binding.value) {
      document.body.classList.add('overflow-hidden');
    } else {
      document.body.classList.remove('overflow-hidden');
    }
  },
  unmounted() {
    document.body.classList.remove('overflow-hidden');
  }
};

export default NoScrollDirective;
