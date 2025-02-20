import { ProviderType } from '@/types/provider';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useProviderStore = defineStore('provider', () => {
    const providers = ref<ProviderType[]>([
        {id: '1067', name: 'Vascular Access Solutions', logo: '', clinicians: '54 clinicians'},
        {id: '1089', name: 'AdventHealth Lake Placid', logo: '', clinicians: '16 clinicians'},
    ]);

    const activeProvider = ref<ProviderType>({
        id: '1067', 
        name: 'Vascular Access Solutions', 
        logo: '', 
        clinicians: '54 clinicians'
    });

    function switchProvider(id: string) {
        const provider = providers.value.find(provider => provider.id === id);
        if (provider) {
            activeProvider.value.id = provider.id;
            activeProvider.value.name = provider.name;
            activeProvider.value.clinicians = provider.clinicians;
            activeProvider.value.logo = provider.logo;
        }
    }

    return { providers, switchProvider, activeProvider };
});
