import './index.css';

import { createApp, provide, h } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import Toast, { PluginOptions, POSITION, TYPE } from 'vue-toastification';
import 'vue-toastification/dist/index.css';

import { DefaultApolloClient } from '@vue/apollo-composable';
import apolloClient from '@/api';

import NoScroll from '@/directives/v-no-scroll';
import { IconSucess, IconError, IconWarning, IconXClose } from '@/components/icons';

const options: PluginOptions = {
  transition: 'Vue-Toastification__slideBlurred',
  containerClassName: 'toast_container',
  closeOnClick: false,
  toastClassName: 'default_toast',
  closeButton: IconXClose,
  hideProgressBar: true,
  position: POSITION.BOTTOM_CENTER,
  toastDefaults: {
    [TYPE.ERROR]: {
      icon: IconError,
    },
    [TYPE.SUCCESS]: {
      icon: IconSucess,
    },
    [TYPE.WARNING]: {
      icon: IconWarning,
    },
    [TYPE.DEFAULT]: {
      hideProgressBar: true,
      icon: false,
    },
  },
};

const app = createApp({
  setup() {
    provide(DefaultApolloClient, apolloClient);
  },
  render: () => h(App),
});

app.use(createPinia());
app.use(router);
app.use(Toast, options);
app.directive('no-scroll', NoScroll);

app.mount('#app');
