import { BreadcrumbItem } from '@/types/breadcrumb'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useBreadcrumbStore = defineStore('breadcrumb', () => {

  const breadcrumbs = ref<BreadcrumbItem[]>([{title: 'Dashboard'}])
  function setBreadcrumbs(crumbs: BreadcrumbItem[]) {
    breadcrumbs.value = crumbs
  }

  return {breadcrumbs, setBreadcrumbs }
})
