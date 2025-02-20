<script setup lang="ts">
import { FwbTextarea, FwbButton } from 'flowbite-vue';
import {
  Dropdown,
  DropdownMenu,
  DropdownItem,
  DropdownDivider,
  DropdownText,
} from '@/components/dropdown/index';
import Modal from '@/components/modal/Modal.vue';
import StatusItem from '@/components/status/StatusItem.vue';
import { computed, onMounted, ref, watch } from 'vue';
import DotIndicator from '@/components/status/DotIndicator.vue';
import ProviderList from './ProviderList.vue';
import { useProviderStore } from '@/stores/provider';
import { ProviderType } from '@/types/provider';
import { useMeStore } from '@/stores/data/settings/users/me';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import { UserAvailability } from '@/api/__generated__/graphql';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const meStore = useMeStore();
const me = computed(() => meStore.me);

const modalRef = ref<InstanceType<typeof Modal>>();
function closeModal() {
  modalRef.value?.setModalOpen(false);
}
function openModal() {
  modalRef.value?.setModalOpen(true);
}

const availibilityStatus = ref<UserAvailability>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

// Reactive schema based on availibilityStatus
const schema = computed(() => {
  return yup.object({
    statusMessage:
      availibilityStatus.value === UserAvailability.Busy ||
      availibilityStatus.value === UserAvailability.Away
        ? yup.string().required('This is a required field')
        : yup.string(),
  });
});

const { handleSubmit } = useForm({
  validationSchema: schema,
  initialValues: { availibilityStatus: availibilityStatus.value }, // Ensure tracking
});

const { value: statusMessage, errorMessage: statusMessageError } =
  useField<string>('statusMessage');

onMounted(() => {
  if (me.value?.status?.status) {
    availibilityStatus.value = me.value.status?.status;
  }
});
watch(
  () => me.value?.status,
  (status) => {
    if (status?.status) {
      availibilityStatus.value = status.status;
    }
  }
);

function changeStatus(status: UserAvailability) {
  availibilityStatus.value = status;
}

function changeStatusMessage() {
  handleSubmit(async () => {
    if (availibilityStatus.value) {
      meStore.modifyMyStatus({
        status: availibilityStatus.value,
        message: statusMessage.value ? statusMessage.value : null,
      });
      modalRef.value?.setModalOpen(false);
    }
  })();
}
function statusModalClosed() {
  if (me.value?.status?.status) {
    availibilityStatus.value = me.value?.status?.status;
  }
  statusMessage.value = '';
}

function resetStatusMessage() {
  if (availibilityStatus.value) {
    meStore.modifyMyStatus({
      status: availibilityStatus.value,
      message: null,
    });
  }
}
const { switchProvider } = useProviderStore();

const modalSwitchProviderRef = ref<InstanceType<typeof Modal>>();
function closeSwitchProviderModal() {
  modalSwitchProviderRef.value?.setModalOpen(false);
}

const activeProviderId = ref('');
function emittedActiveProvider(provider: ProviderType) {
  activeProviderId.value = provider.id;
}

function confirmSwitchProvider() {
  modalSwitchProviderRef.value?.setModalOpen(false);
  if (activeProviderId) {
    switchProvider(activeProviderId.value);
  }
}
</script>
<template>
  <Dropdown align-to-end close-inside>
    <template #trigger>
      <user-avatar
        v-if="me"
        :user="me"
        size="sm"
        rounded
        show-status
        status-position="bottom-right"
        class="cursor-pointer"
      />
    </template>
    <DropdownMenu class="w-80">
      <DropdownText v-if="me" class="py-2">
        <user-avatar :user="me" size="sm" rounded class="mr-4" />
        <div class="">
          <div class="font-semibold text-sm">{{ me?.firstName }} {{ me?.lastName }}</div>
          <div class="flex items-center gap-2">
            <DotIndicator
              v-if="me.status?.status"
              :status="me.status?.status"
              class="border-2 border-white mt-1"
            />
            <span class="text-xm text-slate-500">
              <span class="capitalize">
                {{ me?.status?.status?.toLowerCase() }}
              </span>
              <span v-if="me?.status?.message"> - {{ me.status.message }} </span>
            </span>
          </div>
        </div>
      </DropdownText>
      <DropdownDivider />
      <DropdownItem @click="openModal">
        Change
        <span class="font-bold capitalize"
          >&nbsp; {{ me?.status?.status?.toLowerCase() }} &nbsp;</span
        >
        status</DropdownItem
      >
      <DropdownItem v-if="me?.status?.message" @click="resetStatusMessage">
        Reset Status message
      </DropdownItem>
      <DropdownDivider />
      <DropdownItem to="/user/my-profile"> Profile </DropdownItem>
      <DropdownItem to="/user/preferences">Preferences </DropdownItem>
      <DropdownDivider />
      <DropdownItem @click="modalSwitchProviderRef?.setModalOpen(true)">
        Switch Provider
      </DropdownItem>
      <DropdownDivider />
      <DropdownItem class="!text-radical-red-700"> Sign out of Provider </DropdownItem>
      <DropdownDivider />
      <DropdownText class="justify-end text-xs">LINES ver: v1.20.6</DropdownText>
    </DropdownMenu>
  </Dropdown>

  <Modal ref="modalRef" title="Change Status" @close="statusModalClosed">
    <template #body>
      <div class="flex flex-col gap-4 p-6">
        <StatusItem
          :checked="availibilityStatus === UserAvailability.Free"
          :status="UserAvailability.Free"
          @click="changeStatus(UserAvailability.Free)"
        >
        </StatusItem>
        <StatusItem
          :checked="availibilityStatus === UserAvailability.Busy"
          :status="UserAvailability.Busy"
          @click="changeStatus(UserAvailability.Busy)"
        >
        </StatusItem>
        <StatusItem
          :checked="availibilityStatus === UserAvailability.Away"
          :status="UserAvailability.Away"
          @click="changeStatus(UserAvailability.Away)"
        ></StatusItem>
        <StatusItem
          :checked="availibilityStatus === UserAvailability.Offline"
          :status="UserAvailability.Offline"
          @click="changeStatus(UserAvailability.Offline)"
        >
        </StatusItem>
      </div>
      <hr class="border-slate-200" />
      <div class="p-6">
        <fwb-textarea
          v-model="statusMessage"
          :rows="4"
          label="Your message"
          placeholder="Write your message..."
          :class="statusMessageError ? inputErrorClasses : ''"
        />
        <span v-if="statusMessageError" class="text-sm text-radical-red-600">{{
          statusMessageError
        }}</span>
      </div>
    </template>
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="changeStatusMessage" class="bg-primary-600 hover:bg-brand-600" pill>
          Change Status
        </fwb-button>
      </div>
    </template>
  </Modal>

  <Modal ref="modalSwitchProviderRef" title="Switch Provider">
    <template #body>
      <div class="flex flex-col gap-4 p-6">
        <div class="text-sm font-normal text-slate-500">Some text goes in here.</div>
        <div>
          <ProviderList @switch="emittedActiveProvider" />
        </div>
      </div>
    </template>
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeSwitchProviderModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="confirmSwitchProvider" class="bg-primary-600 hover:bg-brand-600" pill>
          Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
