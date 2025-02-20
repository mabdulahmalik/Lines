<script setup lang="ts">
import { ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { User } from '@/api/__generated__/graphql';
import { useUsersStore } from '@/stores/data/settings/users';

// Props
const props = defineProps<{ user: User }>();

// Stores
const usersStore = useUsersStore();

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();

// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmDeactivate = () => {
  usersStore.deactivateUser({ userId: props.user.id });
  closeModal();
};

// Expose API
defineExpose({
  setModalOpen: (value: boolean) => {
    modalRef.value?.setModalOpen(value);
  },
});
</script>

<template>
  <Modal ref="modalRef" max_width="xl" set_margins>
    <!-- Header -->
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">De-activate User</h3>
    </template>

    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <div class="text-sm font-medium">
          Are you sure you want to de-activate
          <span class="text-purple-800 px-0.5"
            >{{ props.user.firstName }} {{ props.user.lastName }}</span
          >?
        </div>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>Cancel</fwb-button>
        <fwb-button
          @click="confirmDeactivate"
          class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
          pill
        >
          Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
