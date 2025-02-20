// import { useStorage } from '@vueuse/core'
import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export const useSidebarStore = defineStore('sidebar', () => {
  const isSidebarOpen = ref(false)
  const isLgSidebarOpen = ref(true)
  // const selected = useStorage('selected', ref('Job'))
  // const page = useStorage('page', ref('Procedures'))
  const selected = ref('')
  const page = ref('')

  function toggleSidebar() {
    isSidebarOpen.value = !isSidebarOpen.value
    isLgSidebarOpen.value = !isLgSidebarOpen.value
  }
  const isFullWidth = ref(false)


  // Initialized the menu page
  const hasInitialized = ref(false);
  const router = useRouter();
  router.afterEach((to) => {
    if (!hasInitialized.value) {
      if (to.fullPath) {
        const pathParts = to.path.split('/').filter(Boolean);
        page.value = pathParts[0];
        console.log('First route part:', pathParts[0]);
      } else {
        console.log('First route part is undefined');
      }
      hasInitialized.value = true;
    }
  });

  return { isSidebarOpen, toggleSidebar, isLgSidebarOpen, selected, page, isFullWidth }
})
