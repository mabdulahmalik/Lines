<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import { useModalStore } from '@/stores/modal';
import {
  IconArrowLeft,
  IconExpand01,
  IconDotHorizontal,
  IconShrink,
  IconUserX02,
  IconUserCheck02,
} from '@/components/icons/index';
import { User } from '@/api/__generated__/graphql';
import { ref } from 'vue';
import EditUserName from '../EditUserName.vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import DeactivateUserModal from './DeactivateUserModal.vue';
import ActivateUserModal from './ActivateUserModal.vue';

const props = defineProps<{
  fName: string;
  lName: string;
  user: User;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'width', val: string): void;
  (e: 'name-updated', val: { fName: string; lName: string }): void;
}>();

const modalStore = useModalStore();

const fullWidth = ref(false);

const setModalWidth = (val: string) => {
  emit('width', val);
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isModalDrawerExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isModalDrawerExpended = false;
  }
};

const closeModal = () => {
  emit('close');
};
const changeName = (val: { fName: string; lName: string }) => {
  emit('name-updated', val);
};

const deactivateUserModalRef = ref<InstanceType<typeof DeactivateUserModal>>();
const activateUserModalRef = ref<InstanceType<typeof ActivateUserModal>>();
const deactivateUser = () => {
  deactivateUserModalRef.value?.setModalOpen(true);
};
const activateUser = () => {
  activateUserModalRef.value?.setModalOpen(true);
};
</script>

<template>
  <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
    <div class="flex items-center">
      <!-- Mobile close button -->
      <fwb-button
        @click="closeModal"
        color="light"
        pill
        square
        size="lg"
        class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
      >
        <IconArrowLeft width="24" height="24" class="text-[#334155]" />
        <span class="sr-only">Close modal</span>
      </fwb-button>
      <EditUserName :fName="props.fName" :lName="props.lName" @name-updated="changeName" />
    </div>
    <div class="flex items-center gap-2 lg:gap-4">
      <!-- dropdown -->
      <Dropdown align-to-end class="rounded-lg" close-inside>
        <template #trigger>
          <fwb-button color="light" pill square>
            <IconDotHorizontal />
          </fwb-button>
        </template>
        <DropdownMenu>
          <DropdownItem v-if="props.user.active" @click="deactivateUser">
            <IconUserX02 height="20" width="20" stroke-width="2" class="mr-2 text-slate-500" />
            De-activate User
          </DropdownItem>
          <DropdownItem v-if="!props.user.active" @click="activateUser">
            <IconUserCheck02 height="20" width="20" stroke-width="2" class="mr-2 text-slate-500" />
            Activate User
          </DropdownItem>
        </DropdownMenu>
      </Dropdown>

      <!-- Expand -->
      <fwb-button
        v-if="!fullWidth"
        @click="setModalWidth('full')"
        color="light"
        pill
        square
        class="hidden lg:block"
      >
        <IconExpand01 />
      </fwb-button>
      <!-- Shrink -->
      <fwb-button
        v-if="fullWidth"
        @click="setModalWidth('5xl')"
        color="light"
        pill
        square
        class="hidden lg:block"
      >
        <IconShrink />
      </fwb-button>
    </div>
  </div>
  <DeactivateUserModal ref="deactivateUserModalRef" :user="props.user" />
  <ActivateUserModal ref="activateUserModalRef" :user="props.user" />
</template>
