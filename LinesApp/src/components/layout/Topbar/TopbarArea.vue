<script setup lang="ts">
import { useSidebarStore } from "@/stores/sidebar";
import { FwbButton, FwbTooltip } from "flowbite-vue";
import Breadcrumb from "@/components/breadcrumb/index.vue";
import ModalDrawer from "@/components/modal/ModalDrawer.vue";
import UserDropdown from "./UserDropdown.vue";
import { nextTick, ref } from "vue";
import Notification from "./Notification.vue";
import CreateJobModal from "@/views/encounters/jobs/partials/modal/CreateJobModal.vue";
import {
  IconMenu02,
  IconPlus,
  IconMessageQuestionSquare,
  IconBell01,
  IconXClose,
} from "@/components/icons/index";

const isDevMode = import.meta.env.MODE === 'development';

const { toggleSidebar } = useSidebarStore();
const sidebarStore = useSidebarStore();

const modalDrawerRef = ref<InstanceType<typeof ModalDrawer>>();

const createJobModalRef = ref<InstanceType<typeof CreateJobModal>>();

interface Notification {
  date?: string;
  time?: string;
  job_name?: string;
  created_by: string;
  location: string;
}
const notifications = ref<Notification[]>([
  {
    date: "Apr 8, 2024",
    time: "7:50AM",
    job_name: "Job Name",
    created_by: "User Name",
    location: "Location (Facility 1 - Room 3)",
  },
  {
    date: "Apr 8, 2024",
    time: "7:50AM",
    job_name: "Job Name",
    created_by: "User Name",
    location: "Location (Facility 1 - Room 3)",
  },
]);

function removeNotification(index: number) {
  if (index > -1) {
    notifications.value.splice(index, 1);
  }
}
const isCreateJobModalOpen = ref(false);
function openCreateJobModal(){
  isCreateJobModalOpen.value = true;
  nextTick(() => {
    createJobModalRef.value?.setModalOpen(true);
  });
}
</script>

<template>
  <header class="w-full">
    <div
      class="flex items-center top-bar w-full h-16 lg:h-20 fixed bg-header-gradient z-[51]"
    >
      <div
        class="flex items-center w-full justify-between p-4 lg:pl-6 lg:pr-10"
      >
        <div class="flex items-center">
          <fwb-button size="xs" outline square class="hover:bg-transparent border-transparent focus:ring-0">
            <IconMenu02
              class="text-white lg:flex"
              :class="{ hidden: sidebarStore.isSidebarOpen }"
              @click="
                () => {
                  toggleSidebar();
                }
              "
            />
            <IconXClose
              class="text-white lg:hidden"
              :class="{ hidden: !sidebarStore.isSidebarOpen }"
              @click="
                () => {
                  sidebarStore.isSidebarOpen = false;
                }
              "
            />
          </fwb-button>

          <div class="logo mx-3 lg:w-sidebar flex items-center">
            <router-link to="/">
              <img
                src="/lines-logo-white.svg"
                class="h-8 sm:h-11"
                alt="Lines Logo"
              />
            </router-link>
          </div>
          <div class="hidden lg:flex lg:ml-[-40px]">
            <Breadcrumb size="xl" />
          </div>
        </div>

        <!-- Right -->
        <div class="flex items-center gap-4">
          <!-- Create job -->
          <fwb-tooltip>
            <template #trigger>
              <fwb-button
                @click="openCreateJobModal"
                color="light"
                size="xs"
                pill
                square
              >
                <IconPlus class="text-brand-600" />
              </fwb-button>
            </template>
            <template #content> Create Job </template>
          </fwb-tooltip>

          <div class="w-1 h-8 border-r border-white"></div>

          <fwb-button outline square size="xs" class="hover:bg-transparent border-transparent focus:ring-0">
            <IconMessageQuestionSquare class="text-white" />
          </fwb-button>
          <!-- Notifications Button -->
          <fwb-button
            v-if="isDevMode"
            @click="modalDrawerRef?.setModalOpen(true)"
            outline
            square
            size="xs"
            class="hover:bg-transparent border-transparent focus:ring-0"
          >
            <IconBell01 class="text-white" />
          </fwb-button>

          <!-- Usser Dropdown -->
          <div class="">
            <UserDropdown />
          </div>
        </div>
      </div>
    </div>
    <!-- Mobile breadcrum -->
    <div class="lg:hidden relative top-[64px] border-b border-slate-300 p-4">
      <Breadcrumb size="base" color="slate-900" />
    </div>

    <!-- Notification Modal -->
    <ModalDrawer ref="modalDrawerRef" close_outside max_width="lg">
      <template #header>
        <div
          class="flex items-center justify-between pr-4 py-4 lg:pl-6 lg:pr-6"
        >
          <h3 class="text-base lg:text-xl font-semibold text-gray-900">
            Notifications
          </h3>
          <a href="#" class="text-brand-600 text-sm font-medium"
            >Mark all as read</a
          >
        </div>
      </template>
      <template #body class="bg-red-200">
        <div class="flex flex-col">
          <Notification
            v-for="(notification, index) in notifications"
            :notification="notification"
            :key="index"
            :index="index"
            @remove="removeNotification"
          ></Notification>
        </div>
      </template>
    </ModalDrawer>

    <!-- Create Job Modal -->
    <CreateJobModal v-if="isCreateJobModalOpen" ref="createJobModalRef" />
  </header>
</template>

<style scoped></style>
