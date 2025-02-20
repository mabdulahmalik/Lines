<script setup lang="ts">
import { ref, onUnmounted, watch, onMounted } from 'vue';
import { FwbButton } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewUserModalBody from './ViewUserModalBody.vue';
import { useModalStore } from '@/stores/modal';
import { User } from '@/api/__generated__/graphql';
import { useUsersStore } from '@/stores/data/settings/users';
import ViewUserModalHeader from './ViewUserModalHeader.vue';

const props = defineProps<{
  user: User;
}>();

const modalStore = useModalStore();
const usersStore = useUsersStore();


const userFirstName = ref('');
const userLastName = ref('');

onMounted(() => {
  userFirstName.value = props.user.firstName || '';
    userLastName.value = props.user.lastName || '';
});
watch(
  () => props.user,
  (newValue) => {
    userFirstName.value = newValue.firstName || '';
    userLastName.value = newValue.lastName || '';
  }
);
function changeName(val: { fName: string, lName: string }) {
  userFirstName.value = val.fName;
  userLastName.value = val.lName;
}

function saveChanges() {
  console.log('Save changes');
  ViewUserModalBodyRef.value?.modifyUser();
}


// User Modal 
const modalViewUserRef = ref<InstanceType<typeof ModalDrawer>>();
const ViewUserModalBodyRef = ref<InstanceType<typeof ViewUserModalBody>>();

const setModalOpen = (value: boolean): void => {
  modalViewUserRef.value?.setModalOpen(value);
};

function closeUserModal() {
  modalViewUserRef.value?.setModalOpen(false);
  usersStore.clearSelectedUser();
}

const fullWidth = ref(false);
const modalWidth = ref('5xl');
const setModalWidth = (val: string) => {
  modalWidth.value = val;
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isModalDrawerExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isModalDrawerExpended = false;
  }
};
defineExpose({
  setModalOpen,
});
onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});
</script>
<template>
  <ModalDrawer
    ref="modalViewUserRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    @close="usersStore.clearSelectedUser()"
  >
    <template #header>
      <ViewUserModalHeader
        ref="ViewUserModalHeaderRef"
        :fName="userFirstName"
        :lName="userLastName"
        :user="props.user"
        @width="setModalWidth"
        @name-updated="changeName"
        @close="closeUserModal"
      />
    </template>
    <template #body class="bg-red-200">
      <ViewUserModalBody
        ref="ViewUserModalBodyRef"
        :fName="userFirstName"
        :lName="userLastName"
        :user="props.user"
        @close="setModalOpen(false)"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <div class="flex items-center gap-4 lg:gap-6">
          <fwb-button @click="closeUserModal" color="light" pill> Cancel</fwb-button>
          <fwb-button @click="saveChanges" class="bg-primary-600 hover:bg-brand-600" pill>Save</fwb-button>
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
