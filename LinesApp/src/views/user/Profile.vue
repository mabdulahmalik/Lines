<script setup lang="ts">
import { onMounted, onUnmounted, computed, ref, watch } from 'vue';
import Tabs from '@/components/tabs/Tabs.vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import {
  IconUserList,
  IconSecurityPassword,
  IconGoogle,
  IconMicrosoft,
  IconApple,
  IconMail02,
} from '@/components/icons';
import { FwbButton, FwbInput, FwbAvatar } from 'flowbite-vue';
import EmptyStateScreen from '@/components/empty-states/EmptyStateScreen.vue';
import soonSecurityImg from '@/assets/images/user/profile-security.svg';
import { useMeStore } from '@/stores/data/settings/users/me';
import { formatRelativeDate, formatDateByDMY } from '@/utils/dateUtils';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { ModifyMyProfileCmd } from '@/api/__generated__/graphql';
import ProfilePictureModal from './modal/ProfilePictureModal.vue';

const breadcrumbStore = useBreadcrumbStore();
const meStore = useMeStore();


const profilePictureModalRef = ref<InstanceType<typeof ProfilePictureModal>>();
const imageUrl = ref<string | null>(null);
const fileInput = ref<HTMLInputElement | null>(null);

function openFileDialog() {
  if (fileInput.value) {
    fileInput.value.click();
  }
};


onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'User', to: '/user/my-profile' },
    { title: 'My Profile' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const tabs = computed(() => [
  {
    label: 'Information',
    icon: IconUserList,
  },
  {
    label: 'Security',
    icon: IconSecurityPassword,
  },
]);

// Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schema = yup.object({
  firstName: yup.string().required('This is a required field'),
  lastName: yup.string().required('This is a required field'),
  email: yup.string().required('This is a required field'),
  phone: yup.string().required('This is a required field'),
  avatar: yup.string().required('Photo is required'),
});
const { handleSubmit, errors } = useForm({
  validationSchema: schema,
});

// Information
const me = computed(() => meStore.me);

const { value: firstName, errorMessage: firstNameError } = useField<string>('firstName');
const { value: lastName, errorMessage: lastNameError } = useField<string>('lastName');
const { value: email, errorMessage: emailError } = useField<string>('email');
const { value: phone, errorMessage: phoneError } = useField<string>('phone');
const { value: avatar, errorMessage: avatarError } = useField<string>('avatar');

const isSubmitClicked = ref(false);
onMounted(()=>{
  firstName.value = me.value?.firstName || '';
  lastName.value = me.value?.lastName || '';
  email.value = me.value?.email || '';
  avatar.value = me.value?.avatar || 'https://i.pravatar.cc/150?img=1'; //for the moment;
  phone.value = me.value?.phone || '';
})
watch(
  () => me.value,
  (val) => {
    firstName.value = val?.firstName || '';
    lastName.value = val?.lastName || '';
    email.value = val?.email || '';
    avatar.value = val?.avatar || 'https://i.pravatar.cc/150?img=1'; //for the moment
    phone.value = val?.phone || '';
  }
);

const getInitials = computed(()=> {
  if(me.value?.firstName && me.value.lastName){
    return `${me.value.firstName.charAt(0)} ${me.value.lastName.charAt(0)}`;
  }
  return "";
})

function handleSaveChanges() {
  isSubmitClicked.value = true;
  handleSubmit(async () => {
    const updatedUser: ModifyMyProfileCmd = {
      firstName: firstName.value,
      lastName: lastName.value,
      phone: phone.value
    }
    meStore.modifyMyProfile(updatedUser);
  })();
}
function contactSupport() {
  console.log('Contact support');
}

async function handleFileUpload(event: Event) {
  const input = event.target as HTMLInputElement;  
  if (input.files && input.files.length === 1) {
    imageUrl.value = URL.createObjectURL(input.files[0]);
    input.value = "";
    profilePictureModalRef?.value?.setModalOpen(true);
  }
}

async function removeAvatar(){
  await meStore.modifyMyAvatar({
    avatar: null
  });
}
</script>
<template>
  <div class="pt-3 h-full">
    <Tabs :tabs="tabs" margin="lg:mx-6 mx-4">
      <template #tab-0>
        <div class="overflow-y-auto h-[calc(100vh-244px)] lg:h-[calc(100vh-204px)]">
          <div class="max-w-lg mx-auto px-4">
            <div class="mt-4 lg:mt-10 pb-2">
              <div class="font-semibold text-2xl leading-9">Information</div>
              <div class="text-sm font-medium leading-[19.6px]">
                This example text is going to run a bit longer so that you can see how spacing
                within an alert works with this kind of content.
              </div>
            </div>

            <!-- Profile Picture -->
            <div class="border-b border-slate-200 py-6">
              <div class="font-semibold text-base">Profile picture</div>
              <div class="flex gap-4 mt-2">
                <div>
                  <fwb-avatar v-if="me && me.avatar" :img="me.avatar" size="xl" rounded class="custom-size" />
                  <fwb-avatar v-else-if="me && me.firstName && me.lastName" size="xl" rounded :initials="getInitials" class="custom-size" />
                  <div v-else class="w-32 h-32 rounded-full border bg-grays-100"></div>
                </div>
                <div class="flex flex-col gap-4">
                  <fwb-button class="bg-primary-600 hover:bg-brand-600" pill @click="openFileDialog">
                    Upload a new image
                  </fwb-button>
                  <input 
                    type="file" 
                    ref="fileInput" 
                    class="hidden" 
                    @change="handleFileUpload" 
                    accept="image/*" 
                  />
                  <fwb-button
                    color="light"
                    pill
                    class="border-brand-600 text-brand-600 bg-transparent"
                    @click="removeAvatar"
                    :disabled="me?.avatar == null"
                  >
                    Delete image</fwb-button
                  >
                </div>
              </div>
              <span v-if="isSubmitClicked && avatarError" class="text-sm text-radical-red-600 w-full">{{
                avatarError
              }}</span>              
              <ProfilePictureModal ref="profilePictureModalRef" :image-url="imageUrl" />
            </div>

            <!-- Personal Information -->
            <div class="border-b border-slate-200 pb-6 pt-8">
              <div class="font-semibold text-base">Personal Information</div>
              <div class="flex flex-col gap-4 mt-2">
                <div>
                  <label class="mb-1 block text-sm font-medium text-gray-900">First Name</label>
                  <fwb-input v-model="firstName" :class="isSubmitClicked && firstNameError ? inputErrorClasses : ''"/>
                  <span v-if="isSubmitClicked && firstNameError" class="text-sm text-radical-red-600">{{
                    firstNameError
                  }}</span>
                </div>
                <div>
                  <label class="mb-1 block text-sm font-medium text-gray-900">Last Name</label>
                  <fwb-input v-model="lastName" :class="isSubmitClicked && lastNameError ? inputErrorClasses : ''"/>
                  <span v-if="isSubmitClicked && lastNameError" class="text-sm text-radical-red-600">{{
                    lastNameError
                  }}</span>
                </div>
                <div>
                  <label class="mb-1 block text-sm font-medium text-gray-900">Email Address</label>
                  <fwb-input v-model="email" type="email" readonly :class="isSubmitClicked && emailError ? inputErrorClasses : ''"/>
                  <span v-if="isSubmitClicked && emailError" class="text-sm text-radical-red-600">{{
                    emailError
                  }}</span>
                </div>
                <div>
                  <label class="mb-1 block text-sm font-medium text-gray-900"
                    >Phone Number</label
                  >
                  <fwb-input v-model="phone" :class="isSubmitClicked && phoneError ? inputErrorClasses : ''"/>
                  <span v-if="isSubmitClicked && phoneError" class="text-sm text-radical-red-600">{{
                    phoneError
                  }}</span>
                </div>
              </div>
            </div>

            <!-- Activity -->
            <div class="pb-6 pt-8">
              <div class="font-semibold text-base">Activity</div>
              <div class="flex flex-col gap-4 mt-2">
                <div class="flex items-center gap-2">
                  <label class="w-2/5 text-sm font-medium text-slate-500">Registration</label>
                  <div class="text-sm font-semibold text-slate-900">{{ formatDateByDMY(me?.registeredAt) }}</div>
                </div>
                <div class="flex items-center gap-2">
                  <label class="w-2/5 text-sm font-medium text-slate-500">Last Login</label>
                  <div class="text-sm font-semibold text-slate-900">{{ formatRelativeDate(me?.loggedInAt) }}</div>
                </div>
                <div class="flex items-center gap-2">
                  <label class="w-2/5 text-sm font-medium text-slate-500"
                    >Last Password Change</label
                  >
                  <div class="text-sm font-semibold text-slate-900">{{ formatDateByDMY(me?.passwordChangedAt) }}</div>
                </div>
                <div class="flex items-center gap-2">
                  <label class="w-2/5 text-sm font-medium text-slate-500">Role(s)</label>
                  <div class="flex gap-4 flex-wrap">
                    <span
                      v-for="(role, i) in me?.roles"
                      :key="i"
                      class="py-0.5 px-[10px] rounded-full text-xs font-medium"
                      :class="{
                        'bg-green-100 text-green-600 ': role?.name === 'Administrator',
                        'bg-brand-100 text-brand-700 ': role?.name === 'Director',
                        'bg-blue-100 text-blue-700 ': role?.name === 'Clinician',
                      }"
                      >{{ role?.name }}
                    </span>
                  </div>
                </div>
                <div class="flex items-center gap-2">
                  <label class="w-2/5 text-sm font-medium text-slate-500">Identity Provider</label>
                  <div>
                    <span v-if="me?.identity === 'goolge'">
                      <IconGoogle class="w-5 h-5" />
                    </span>
                    <span v-else-if="me?.identity === 'microsoft'">
                      <IconMicrosoft class="w-5 h-5" />
                    </span>
                    <span v-else-if="me?.identity === 'apple'">
                      <IconApple class="w-5 h-5" />
                    </span>
                    <span v-else-if="me?.identity === 'email'">
                      <IconMail02 class="w-5 h-5" />
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="sticky bottom-0 bg-white border-t border-slate-200">
          <div class="px-6 py-4 items-center flex justify-end">
            <span
              v-if="isSubmitClicked && Object.keys(errors).length"
              class="text-sm font-normal text-radical-red-600 mr-4"
              >{{ Object.keys(errors).length }} errors</span
            >
            <fwb-button @click="handleSaveChanges" class="bg-primary-600 hover:bg-brand-600" pill> Save Changes </fwb-button>
          </div>
        </div>
      </template>
      <template #tab-1>
        <div class="overflow-y-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-164px)]">
          <div class="max-w-md mx-auto">
            <EmptyStateScreen
              title="Security settings coming soon"
              description="Please contact support of they need to change their password or their MFA settings."
              buttonText="Contact support"
              @action="contactSupport"
              class="overflow-hidden"
            >
              <template #image>
                <img :src="soonSecurityImg" alt="comming soon" />
              </template>
            </EmptyStateScreen>
          </div>
        </div>
      </template>
    </Tabs>
  </div>
</template>

<style scoped>
:deep(.custom-size > .relative){
  width: 8rem;
  height: 8rem;
}
</style>
